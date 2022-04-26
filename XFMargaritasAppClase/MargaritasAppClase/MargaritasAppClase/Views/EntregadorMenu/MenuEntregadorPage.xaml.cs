using MargaritasAppClase.Controller;
using MargaritasAppClase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Plugin.LocalNotification;
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
using static MargaritasAppClase.Messages;

namespace MargaritasAppClase.Views.EntregadorMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuEntregadorPage : ContentPage
    {

        string cliente, ordenCorrelativo;

        List<EntregadorListPedidosModel> listaordenesrepartidor = null;

        string correo = Application.Current.Properties["correo"].ToString();

        public MenuEntregadorPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                MessagingCenter.Subscribe<LocationMessage>(this, "Location", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        //locationLabel.Text += $"{Environment.NewLine}{message.Latitude}, {message.Longitude}, {DateTime.Now.ToLongTimeString()}";

                        Console.WriteLine($"{message.Latitude}, {message.Longitude}, {DateTime.Now.ToLongTimeString()}");
                        UpdateOrderLocation(message.Latitude.ToString(), message.Longitude.ToString());
                    });
                });

                MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        //locationLabel.Text = "El servicio de ubicacion, se ha detenido!";
                    });
                });

                MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        //locationLabel.Text = "There was an error updating location!";
                    });
                });


                if (Preferences.Get("LocationServiceRunning", false) == true)
                {
                    //StartService();
                    //StopService();
                }

            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetOrdenesRepartidorList();
        }

        private async void GetOrdenesRepartidorList()
        {
            try
            {
                var AccesoInternet = Connectivity.NetworkAccess;

                if (AccesoInternet == NetworkAccess.Internet)
                {
                    slrepartidormainpage.IsVisible = true;
                    spinnerrepartidormainpage.IsRunning = true;

                    listaordenesrepartidor = new List<EntregadorListPedidosModel>();
                    listaordenesrepartidor = await ProductsApiController.ControllerObtenerListaOrdenesEntregador(correo);

                    if (listaordenesrepartidor.Count > 0)
                    {
                        listview_mainproductosentregador.ItemsSource = null;
                        listview_mainproductosentregador.ItemsSource = listaordenesrepartidor;
                    }
                    else
                    {
                        await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                    }

                    slrepartidormainpage.IsVisible = false;
                    spinnerrepartidormainpage.IsRunning = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void btnverorden_Clicked(object sender, EventArgs e)
        {
            var item = (sender as Button).BindingContext as EntregadorListPedidosModel;

            string correoCliente, ordenNumero, fecha, direccion, audio;
            correoCliente = item.id_cliente;
            ordenNumero = item.ult_cor_pedido;
            fecha = item.fh_entrega;
            direccion = item.direccion;
            audio = item.audio;

            await Navigation.PushAsync(new EntregadorDetallePedidoPage(ordenNumero, fecha, direccion, correoCliente, audio));
        }

        private async void btncambiarstatusorden_Clicked(object sender, EventArgs e)
        {
            var item = (sender as Button).BindingContext as EntregadorListPedidosModel;
            ordenCorrelativo = item.ult_cor_pedido;
            string orden = item.id_pedido;
            cliente = item.id_cliente;
            string estado = item.ID_Estado;

            var alert = await DisplayAlert("Margaritas App", "Seleccione el estatus de la orden", "Orden Entregada", "En Proceso");

            if (alert)
            {
                if (estado == "3")
                {
                    CambiarStatusOrdenModel save = new CambiarStatusOrdenModel
                    {
                        Action = "est",
                        ID_Cliente = cliente,
                        Correl = ordenCorrelativo,
                        Estado = "4"
                    };

                    Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/");

                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(save);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        String jsonx = response.Content.ReadAsStringAsync().Result;
                        JObject jsons = JObject.Parse(jsonx);
                        String Mensaje = jsons["msg"].ToString();
                        await DisplayAlert("Success", "Orden " + orden + " Entregada", "Ok");
                        GetOrdenesRepartidorList();


                        var notificacion = new NotificationRequest
                        {
                            BadgeNumber = 1,
                            Title = "Status de Orden",
                            Description = "Orden " + orden + " entregada, solicitar valoración",
                            ReturningData = "Dummy Data",
                            NotificationId = 1337,

                        };
                        await NotificationCenter.Current.Show(notificacion);


                        var permission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>();

                        if (permission == Xamarin.Essentials.PermissionStatus.Denied)
                        {
                            // TODO Let the user know they need to accept
                            return;
                        }

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            if (CrossGeolocator.Current.IsListening)
                            {
                                await CrossGeolocator.Current.StopListeningAsync();
                                CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;

                                return;
                            }

                            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 10, false, new Plugin.Geolocator.Abstractions.ListenerSettings
                            {
                                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                                AllowBackgroundUpdates = true,
                                DeferLocationUpdates = false,
                                DeferralDistanceMeters = 10,
                                DeferralTime = TimeSpan.FromSeconds(5),
                                ListenForSignificantChanges = true,
                                PauseLocationUpdatesAutomatically = true
                            });

                            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            if (Preferences.Get("LocationServiceRunning", false) == true)
                            {
                                //StartService();
                                StopService();
                            }
                            //else
                            //{
                            //    StopService();
                            //}
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "Solo se pueden cerrar ordenes en proceso", "Ok");
                }
            }
            else
            {
                if(estado == "2")
                {
                    CambiarStatusOrdenModel save = new CambiarStatusOrdenModel
                    {
                        Action = "est",
                        ID_Cliente = cliente,
                        Correl = ordenCorrelativo,
                        Estado = "3"
                    };

                    Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/");

                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(save);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        String jsonx = response.Content.ReadAsStringAsync().Result;
                        JObject jsons = JObject.Parse(jsonx);
                        String Mensaje = jsons["msg"].ToString();
                        await DisplayAlert("Success", "Orden " + orden + " en Proceso", "Ok");
                        GetOrdenesRepartidorList();

                        var notificacion = new NotificationRequest
                        {
                            BadgeNumber = 1,
                            Title = "Status de Orden",
                            Description = "Orden " + orden + " en proceso, favor llegar a tiempo",
                            ReturningData = "Dummy Data",
                            NotificationId = 1337,

                        };
                        await NotificationCenter.Current.Show(notificacion);


                        var permission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>();

                        if (permission == Xamarin.Essentials.PermissionStatus.Denied)
                        {
                            // TODO Let the user know they need to accept
                            return;
                        }

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            if (CrossGeolocator.Current.IsListening)
                            {
                                await CrossGeolocator.Current.StopListeningAsync();
                                CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;

                                return;
                            }

                            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 10, false, new Plugin.Geolocator.Abstractions.ListenerSettings
                            {
                                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                                AllowBackgroundUpdates = true,
                                DeferLocationUpdates = false,
                                DeferralDistanceMeters = 10,
                                DeferralTime = TimeSpan.FromSeconds(5),
                                ListenForSignificantChanges = true,
                                PauseLocationUpdatesAutomatically = true
                            });

                            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            if (Preferences.Get("LocationServiceRunning", false) == false)
                            {
                                StartService();
                            }
                            //else
                            //{
                            //    StopService();
                            //}
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "Estatus solo es valido, para ordenes asignadas", "Ok");
                }
            }
        }

        private async void btnverubiacionorden_Clicked(object sender, EventArgs e)
        {

            var item = (sender as Button).BindingContext as EntregadorListPedidosModel;
            double lat = Convert.ToDouble(item.latitud.ToString());
            double lon = Convert.ToDouble(item.longitud.ToString());
            await Navigation.PushAsync(new MapaEntregadorPage(lat, lon));
        }

        private async void btnverperfilcliente_Clicked(object sender, EventArgs e)
        {
            var item = (sender as Button).BindingContext as EntregadorListPedidosModel;
            string correoClientePedido = item.id_cliente.ToString();
            await Navigation.PushAsync(new VerPerfilClientePage(correoClientePedido));
        }


        private void StartService()
        {
            var startServiceMessage = new StartServiceMessage();
            MessagingCenter.Send(startServiceMessage, "ServiceStarted");
            Preferences.Set("LocationServiceRunning", true);
            //locationLabel.Text = "Location Service has been started!";
        }

        private void StopService()
        {
            var stopServiceMessage = new StopServiceMessage();
            MessagingCenter.Send(stopServiceMessage, "ServiceStopped");
            Preferences.Set("LocationServiceRunning", false);
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            //locationLabel.Text += $"{e.Position.Latitude}, {e.Position.Longitude}, {e.Position.Timestamp.TimeOfDay}{Environment.NewLine}";

            Console.WriteLine($"{e.Position.Latitude}, {e.Position.Longitude}, {e.Position.Timestamp.TimeOfDay}");
        }


        private async void UpdateOrderLocation(string latitud, string longitud)
        {
            try
            {
                ActualizarUbicacionPedidoModel save = new ActualizarUbicacionPedidoModel
                {
                    Action = "upd",
                    ID_Cliente = cliente,
                    Correl = ordenCorrelativo,
                    LatitudPed = latitud,
                    LongitudPed = longitud
                };

                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(save);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["msg"].ToString();
                    //await DisplayAlert("Success", "Orden " + orden + " en Proceso", "Ok");
                    Console.WriteLine($"Ubicacion de la orden; " + ordenCorrelativo + ", actualizada");
                    GetOrdenesRepartidorList();
                }
                else
                {
                    Console.WriteLine($"No se pudo actualizar la ubicacion de la orden; " + ordenCorrelativo);
                    //await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: " + ex.ToString());
            }

        }
    }
}