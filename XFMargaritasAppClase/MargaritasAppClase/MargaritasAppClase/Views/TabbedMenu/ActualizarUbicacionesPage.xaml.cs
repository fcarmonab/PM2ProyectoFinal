using MargaritasAppClase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views.TabbedMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarUbicacionesPage : ContentPage
    {
        public ActualizarUbicacionesPage(byte[] newBytes)
        {
            InitializeComponent();
            this.imageToSave = newBytes;
            obtenerCoordenadas();
        }
        byte[] imageToSave;
        string correo = Application.Current.Properties["correo"].ToString();
        public string AudioPath, fileName;

        private async void btnactualizarphotoubicacion_Clicked(object sender, EventArgs e)
        {
            try
            {
                var takepic = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "PhotoApp",
                    Name = DateTime.Now.ToString() + "_Pic.jpg",
                    SaveToAlbum = true,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                    CompressionQuality = 40
                });

                //  await DisplayAlert("Ubicacion de la foto: ", takepic.Path, "Ok");

                if (takepic != null)
                {
                    imageToSave = null;
                    MemoryStream memoryStream = new MemoryStream();

                    takepic.GetStream().CopyTo(memoryStream);
                    imageToSave = memoryStream.ToArray();

                    imgubicacion.Source = ImageSource.FromStream(() => { return takepic.GetStream(); });
                }

                //descripcion_entry.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void btnactualizarubicacion_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(actualizardescripcion_input.Text))
                {
                    await DisplayAlert("Campo Vacio", "Por favor, Complete los campos requeridos ", "Ok");
                }
                else
                {
                    //convertir la imagen a base64
                    string pathBase64Imagen = Convert.ToBase64String(imageToSave);

                    ActualizarUbicacionModel save = new ActualizarUbicacionModel
                    {
                        ID_Ubicacion = id.Text,
                        ID_Cliente = correo,
                        Latitud = actualizarlatitud_input.Text,
                        Longitud = actualizarlongitud_input.Text,
                        Direccion = actualizardescripcion_input.Text,
                        Foto = pathBase64Imagen,
                        Nota = "",
                        Action = "upd"
                    };

                    Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/ubicaciones/");

                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(save);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        String jsonx = response.Content.ReadAsStringAsync().Result;
                        JObject jsons = JObject.Parse(jsonx);
                        String Mensaje = jsons["msg"].ToString();

                        await DisplayAlert("Success", "Datos actualizados correctamente", "Ok");
                        await Navigation.PopAsync();
                    }

                    else
                    {
                        await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public async void obtenerCoordenadas()
        {
            try
            {
                var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                var tokendecancelacion = new CancellationTokenSource();

                var localizacion = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);


                if (localizacion != null)
                {
                    actualizarlatitud_input.Text = localizacion.Latitude.ToString();
                    actualizarlongitud_input.Text = localizacion.Longitude.ToString();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Advertencia", "Este dispositivo no soporta GPS", "Ok");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Advertencia", "Error de Dispositivo", "Ok");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Advertencia", "Sin Permisos de Geolocalizacion", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Advertencia", "Sin Ubicacion", "Ok");
            }
        }

    }
}