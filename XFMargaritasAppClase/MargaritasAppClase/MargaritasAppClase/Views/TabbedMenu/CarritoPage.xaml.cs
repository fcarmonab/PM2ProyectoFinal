using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MargaritasAppClase.Controller;
using MargaritasAppClase.Models;
using Xamarin.Essentials;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using Plugin.AudioRecorder;
using Plugin.Media;
using System.IO;

namespace MargaritasAppClase.Views.TabbedMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarritoPage : ContentPage
    {

        List<CarritoListModel> listacarrito = null;
        List<UbicacionesListModel> listaubicaciones = null;

        public string AudioPath, fileName;
        private readonly AudioRecorderService audioRecorderService = new AudioRecorderService
        {
            StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
            StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
            TotalAudioTimeout = TimeSpan.FromSeconds(180) //audio will stop recording after 3 minutes
        };

        private readonly AudioPlayer audioPlayer = new AudioPlayer();
        byte[] imageToSave, audioToSave;

        string ubicacionId;
        Object objSitioGlobal = null;
        string correo = Application.Current.Properties["correo"].ToString();
        public CarritoPage()
        {
            InitializeComponent();
            //GetCarritoList();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetCarritoList();
            datefechadeentrega.MinimumDate = new DateTime(Convert.ToInt32(DateTime.Now.ToString("yyyy")), Convert.ToInt32(DateTime.Now.ToString("MM")), Convert.ToInt32(DateTime.Now.ToString("dd")));
            datefechadeentrega.MaximumDate = new DateTime(Convert.ToInt32(DateTime.Now.ToString("yyyy")) + 1, 1, 31);
            timehoraentrega.Time = new TimeSpan(Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), Convert.ToInt32(DateTime.Now.ToString("ss")));
        }

        private async void btnagregarproductocarrito_Clicked(object sender, EventArgs e)
        {
            //var item = (sender as Button).BindingContext as CarritoListModel;
            //await DisplayAlert("Success", "Item: " + item.desc_prod.ToString() + ", Modificado", "Ok");

            try
            {
                var item = (sender as Button).BindingContext as CarritoListModel;
                //await DisplayAlert("Aviso", item.Descripcion.ToString(), "Ok");

                int nuevacantidad = Convert.ToInt32(item.Cantidad.ToString()) + 1;
                double nuevoTotal = Convert.ToInt32(item.Precio.ToString()) * nuevacantidad;

                var listDetalle = new List<CarritoDetalleModel>();
                listDetalle.Add(new CarritoDetalleModel()
                {
                    ID_Producto = item.ID_Producto.ToString(),
                    Cantidad = nuevacantidad.ToString(),
                    Precio = item.Precio.ToString(),
                    Total = nuevoTotal.ToString("#.00")
                });

                SaveCarritoModel jsonObject = new SaveCarritoModel();
                jsonObject.Carrito = new List<CarritoEncabezadoModel>();

                double ldImpueto = 0.00, ldTotal = 0.00;
                ldImpueto = nuevoTotal * .15;
                ldTotal = nuevoTotal + ldImpueto;
                string totImpueto = "", totCarrito = "";
                totImpueto = ldImpueto.ToString("0.##");
                totCarrito = ldTotal.ToString("0.##");

                jsonObject.Carrito.Add(new CarritoEncabezadoModel()
                {

                    ID_Cliente = correo,
                    Fecha = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                    SubTotal = nuevoTotal.ToString("#.00"),
                    ISV = totImpueto,
                    Total = totCarrito,
                    DetalleCarrito = listDetalle

                });


                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/carrito/carritoapp.php");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(jsonObject);

                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["msg"].ToString();
                    await DisplayAlert("Success", "Cantidad del item: " + item.desc_prod.ToString() + ", actualizada", "Ok");
                    GetCarritoList();

                }
                else
                {
                    await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                }

                //await DisplayAlert("Aviso", json, "Ok");                

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Ok");
            }

        }

        private async void btnrestarproductocarrito_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (sender as Button).BindingContext as CarritoListModel;
                //await DisplayAlert("Aviso", item.Descripcion.ToString(), "Ok");

                if (Convert.ToInt32(item.Cantidad.ToString()) > 1)
                {
                    int nuevacantidad = Convert.ToInt32(item.Cantidad.ToString()) - 1;
                    double nuevoTotal = Convert.ToInt32(item.Precio.ToString()) * nuevacantidad;

                    var listDetalle = new List<CarritoDetalleModel>();
                    listDetalle.Add(new CarritoDetalleModel()
                    {
                        ID_Producto = item.ID_Producto.ToString(),
                        Cantidad = nuevacantidad.ToString(),
                        Precio = item.Precio.ToString(),
                        Total = nuevoTotal.ToString("#.00")
                    });

                    SaveCarritoModel jsonObject = new SaveCarritoModel();
                    jsonObject.Carrito = new List<CarritoEncabezadoModel>();

                    double ldImpueto = 0.00, ldTotal = 0.00;
                    ldImpueto = nuevoTotal * .15;
                    ldTotal = nuevoTotal + ldImpueto;
                    string totImpueto = "", totCarrito = "";
                    totImpueto = ldImpueto.ToString("0.##");
                    totCarrito = ldTotal.ToString("0.##");

                    jsonObject.Carrito.Add(new CarritoEncabezadoModel()
                    {

                        ID_Cliente = correo,
                        Fecha = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                        SubTotal = nuevoTotal.ToString("#.00"),
                        ISV = totImpueto,
                        Total = totCarrito,
                        DetalleCarrito = listDetalle

                    });


                    Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/carrito/carritoapp.php");

                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(jsonObject);

                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        String jsonx = response.Content.ReadAsStringAsync().Result;
                        JObject jsons = JObject.Parse(jsonx);
                        String Mensaje = jsons["msg"].ToString();
                        await DisplayAlert("Success", "Cantidad del item: " + item.desc_prod.ToString() + ", actualizada", "Ok");
                        GetCarritoList();

                    }
                    else
                    {
                        await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                    }

                    //await DisplayAlert("Aviso", json, "Ok");                
                }
                else
                {
                    await DisplayAlert("Alerta", "La cantidad debe ser mayor a cero", "Ok");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Ok");
            }

        }

        private async void btnquitarproductocarrito_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (sender as Button).BindingContext as CarritoListModel;

                EliminarItemCarritoModel save = new EliminarItemCarritoModel
                {
                    ID_Cliente = correo,
                    ID_Producto = item.ID_Producto
                };

                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/carrito/del.php");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(save);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["msg"].ToString();

                    await DisplayAlert("Success", "Item: " + item.desc_prod.ToString() + ", eliminado", "Ok");
                    GetCarritoList();
                }

                else
                {
                    await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }

        private async void btnaudioreferencia_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();

            if (status != PermissionStatus.Granted)
                return;

            if (audioRecorderService.IsRecording)
            {
                await audioRecorderService.StopRecording();
                getRecord();
            }
            else
            {
                await audioRecorderService.StartRecording();
            }
        }

        private async void GetCarritoList()
        {
            var AccesoInternet = Connectivity.NetworkAccess;

            if (AccesoInternet == NetworkAccess.Internet)
            {
                sl.IsVisible = true;
                spinner.IsRunning = true;

                listacarrito = new List<CarritoListModel>();
                listacarrito = await ProductsApiController.ControllerObtenerListaCarrito(correo);

                listview_carritoproductos.ItemsSource = null;
                double subtotal = 0, impuesto = 0, total = 0;

                if (listacarrito.Count > 0)
                {
                    //listview_carritoproductos.ItemsSource = null;
                    listview_carritoproductos.ItemsSource = listacarrito;                        

                    foreach (var v in listacarrito)
                    {
                        subtotal = subtotal + Convert.ToDouble(v.Cantidad.ToString()) * Convert.ToDouble(v.Precio.ToString());
                    }
                    impuesto = subtotal * .15;
                    total = subtotal + impuesto;
                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, agregue un item al carrito", "Ok");
                }

                lblsubtotal.Text = "L. " + subtotal.ToString("#,#.00");
                lblisv.Text = "L. " + impuesto.ToString("#,#.00");
                lbltotalapagar.Text = "L. " + total.ToString("#,#.00");

                listaubicaciones = new List<UbicacionesListModel>();
                listaubicaciones = await ProductsApiController.ControllerObtenerListaUbicaciones(correo);

                if (listaubicaciones.Count > 0)
                {

                    selectubicacion.ItemsSource = null;
                    //selectubicacion.ItemsSource = listacarrito;                    

                    var pickerList = new List<String>();

                    foreach (var v in listaubicaciones)
                    {
                        pickerList.Add(v.ID_Ubicacion.ToString() + "-" + v.Direccion.ToString());
                    }

                    selectubicacion.ItemsSource = pickerList;
                    selectubicacion.SelectedIndex = 0;
                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, agregue una ubicación", "Ok");
                }

                sl.IsVisible = false;
                spinner.IsRunning = false;
            }

        }

        private void datefechadeentrega_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private async void selectubicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker pickerUbicacion = sender as Picker;
                var selectedItem = pickerUbicacion.SelectedItem;
                var itemValue = selectedItem.ToString().Split('-').First();
                var timeSpan = timehoraentrega.Time;
                ubicacionId = itemValue;
            }
            catch(OperationCanceledException ocEx)
            {

            }catch(Exception ex)
            {

            }
        }

        private async void btnagregarubicacioncarrito_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaUbicacionesPage());
        }

        private async void btnprocesarorden_Clicked(object sender, EventArgs e)
        {
            if (listacarrito.Count > 0)
            {
                if(listaubicaciones.Count > 0)
                {
                    if (Convert.ToInt32(timehoraentrega.Time.ToString("hh")) < Convert.ToInt32(DateTime.Now.ToString("hh")) + 1)
                    {
                        await DisplayAlert("Aviso", $"Su orden debe tener minimo 1 hora de anticipacion", "Ok");
                    }
                    else
                    {
                        string audio, pathBase64Audio;
                        if (String.IsNullOrEmpty(AudioPath))
                        {
                            pathBase64Audio = "";
                        }
                        else
                        {
                            //extraer el path del audio
                            audio = AudioPath;
                            //convertir a arreglo de bytes
                            byte[] fileByte = System.IO.File.ReadAllBytes(audio);
                            //convertir el audio a base64
                            pathBase64Audio = Convert.ToBase64String(fileByte);
                        }

                        await Navigation.PushAsync(new PagoOrdenPage(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), datefechadeentrega.Date.ToString("yyyy/MM/dd") + " " + timehoraentrega.Time.ToString(), ubicacionId, pathBase64Audio, listacarrito, lbltotalapagar.Text));
                    }
                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, agregue una ubicación", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Notificación", $"Lista vacía, agregue un item al carrito", "Ok");
            }

        }

        private async void getRecord()
        {
            if (audioRecorderService.FilePath != null)
            {
                var stream = audioRecorderService.GetAudioFileStream();

                fileName = Path.Combine(FileSystem.CacheDirectory, DateTime.Now.ToString("ddMMyyyymmss") + "_VoiceNote.wav");

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                    audioToSave = null;
                    MemoryStream memoryStream = new MemoryStream();
                    audioToSave = memoryStream.ToArray();
                }

                await DisplayAlert("Alerta", fileName, "cancel");

                AudioPath = fileName;

            }

        }

    }
}