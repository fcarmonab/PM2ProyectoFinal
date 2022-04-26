using MargaritasAppClase.Controller;
using MargaritasAppClase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
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
    public partial class BusquedaPage : ContentPage
    {
        List<ClienteListaPedidosModel> listaordenescliente = null;

        string correo = Application.Current.Properties["correo"].ToString();

        public BusquedaPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetOrdenesClienteList();
        }

        private async void btnubicacionpedido_Clicked(object sender, EventArgs e)
        {
            var item = (sender as Button).BindingContext as ClienteListaPedidosModel;
            double lat = Convert.ToDouble(item.latitud.ToString());
            double lon = Convert.ToDouble(item.longitud.ToString());
            double latped = Convert.ToDouble(item.latitudped.ToString());
            double lonped = Convert.ToDouble(item.longitudped.ToString());

            await Navigation.PushAsync(new MapaSeguirPedidoPage(lat, lon, latped, lonped));
        }

        private async void btnverdetallepedido_Clicked(object sender, EventArgs e)
        {
            var item = (sender as Button).BindingContext as ClienteListaPedidosModel;
            string correlativo = item.ult_cor_pedido;
            string fecha = item.fh_entrega;            
            string audio = item.audio;
            string direccion = item.direccion;

            await Navigation.PushAsync(new DetallePedidoPage(correlativo,fecha,direccion,audio));
        }

        private async void btnevaluarservicio_Clicked(object sender, EventArgs e)
        {
            var item = (sender as Button).BindingContext as ClienteListaPedidosModel;
            string correlativo = item.ult_cor_pedido;
            string estado = item.ID_Estado;

            if(estado == "4")
            {
                await Navigation.PushAsync(new EvaluacionPage(correlativo));
            }
            else
            {
                await DisplayAlert("Aviso", "Solo se puede valorar las ordenes entregadas", "Ok");
            }
            
        }


        private async void GetOrdenesClienteList()
        {
            try
            {
                var AccesoInternet = Connectivity.NetworkAccess;

                if (AccesoInternet == NetworkAccess.Internet)
                {
                    sl_historialpedidos.IsVisible = true;
                    spinner_historialpedidos.IsRunning = true;

                    listaordenescliente = new List<ClienteListaPedidosModel>();
                    listaordenescliente = await ProductsApiController.ControllerObtenerListaOrdenesCliente(correo);

                    string vestado = "", vnotiproceso = "", vnotientregado = "", vcorrelativo = "", vorden = "";

                    if (listaordenescliente.Count > 0)
                    {
                        listview_historialpedidos.ItemsSource = null;
                        listview_historialpedidos.ItemsSource = listaordenescliente;

                        foreach (var v in listaordenescliente)
                        {                            
                            vestado = v.ID_Estado;
                            vnotiproceso = v.NotiProceso;
                            vnotientregado = v.NotiEntregado;
                            vcorrelativo = v.ult_cor_pedido;
                            vorden = v.id_pedido;

                            if(vestado == "3")
                            {
                                if (vnotiproceso == "0")
                                {
                                    var notificacion = new NotificationRequest
                                    {
                                        BadgeNumber = 1,
                                        Title = "Status de Orden",
                                        Description = "Orden " + vorden + " en camino, por favor estar pendiente",
                                        ReturningData = "Dummy Data",
                                        NotificationId = 1337,
                                    };
                                    await NotificationCenter.Current.Show(notificacion);

                                    // cambiamos la bandera de notificacion proceso
                                    CambiarBanderaOrdenesNotificacionModel save = new CambiarBanderaOrdenesNotificacionModel
                                    {
                                        Action = "not",
                                        ID_Cliente = correo,
                                        Correl = vcorrelativo,
                                        Nproceso = "1",
                                        Npedido = "0"
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
                                    }
                                }
                            }
                            else if (vestado == "4")
                            {
                                if (vnotientregado == "0")
                                {
                                    var notificacion = new NotificationRequest
                                    {
                                        BadgeNumber = 1,
                                        Title = "Status de Orden",
                                        Description = "Orden " + vorden + " entregada, gracias por su preferencia",
                                        ReturningData = "Dummy Data",
                                        NotificationId = 1337,
                                    };
                                    await NotificationCenter.Current.Show(notificacion);

                                    // cambiamos la bandera de notificacion proceso
                                    CambiarBanderaOrdenesNotificacionModel save = new CambiarBanderaOrdenesNotificacionModel
                                    {
                                        Action = "not",
                                        ID_Cliente = correo,
                                        Correl = vcorrelativo,
                                        Nproceso = "1",
                                        Npedido = "1"
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
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                    }

                    sl_historialpedidos.IsVisible = false;
                    spinner_historialpedidos.IsRunning = false;
                }
            }
            catch (Exception ex)
            {

            }

        }

    }
}