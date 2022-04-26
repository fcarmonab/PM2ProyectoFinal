using MargaritasAppClase.Controller;
using MargaritasAppClase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views.TabbedMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaUbicacionesPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        Object objSitioGlobal = null;
        byte[] newBytes = null;
        public ListaUbicacionesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetUbicacionesList();
        }

        private async void tbiagregarubicacionespage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgregarUbicacionesPage());
        }

        private void ls_ubicaciones_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void GetUbicacionesList()
        {
            var AccesoInternet = Connectivity.NetworkAccess;

            if (AccesoInternet == NetworkAccess.Internet)
            {
                slubicaciones.IsVisible = true;
                spinnerubicaciones.IsRunning = true;

                List<UbicacionesListModel> listaubicaciones = new List<UbicacionesListModel>();
                listaubicaciones = await ProductsApiController.ControllerObtenerListaUbicaciones(correo);

                if (listaubicaciones.Count > 0)
                {
                    ls_ubicaciones.ItemsSource = null;
                    ls_ubicaciones.ItemsSource = listaubicaciones;
                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                }

                slubicaciones.IsVisible = false;
                spinnerubicaciones.IsRunning = false;
            }
        }

        private async void ls_ubicaciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                UbicacionesListModel ubicacionesListModel = (UbicacionesListModel)e.Item;

                var messageAlert = await DisplayAlert("Opción", "Seleccione una opción", "Editar ubicación", "Eliminar Ubicación");

                if (messageAlert)
                {
                    string img64 = ubicacionesListModel.Foto.ToString();
                    newBytes = Convert.FromBase64String(img64);
                    var stream = new MemoryStream(newBytes);
                    // editar ubicacion
                    var changeUbicacionBinding = new UbicacionModel
                    {
                        ID_Ubicacion = ubicacionesListModel.ID_Ubicacion,
                        ID_Cliente = ubicacionesListModel.ID_Cliente,
                        Latitud = ubicacionesListModel.Latitud,
                        Longitud = ubicacionesListModel.Longitud,
                        Direccion = ubicacionesListModel.Direccion,
                        Foto = ubicacionesListModel.Foto,
                        Nota = ubicacionesListModel.Nota,
                        Action = "",
                        fotografia = ImageSource.FromStream(() => stream)
                    };

                    var openPageActualizarUbicacion = new ActualizarUbicacionesPage(newBytes);
                    openPageActualizarUbicacion.BindingContext = changeUbicacionBinding;

                    await Navigation.PushAsync(openPageActualizarUbicacion);

                }
                else
                {                    
                    try
                    {
                        EliminarUbicacionModel save = new EliminarUbicacionModel
                        {
                            ID_Ubicacion = ubicacionesListModel.ID_Ubicacion,
                            ID_Cliente = correo,
                            Action = "del"
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

                            await DisplayAlert("Success", "Ubicación eliminada correctamente", "Ok");
                            GetUbicacionesList();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                        }                        
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "Ok");
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.Message, "Ok");
            }
        }
    }
}