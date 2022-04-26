using MargaritasAppClase.Controller;
using MargaritasAppClase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class PagoOrdenPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        string tarjetaId;
        public PagoOrdenPage(string fechaPedido, string fechaEntrega, string idUbicacion, string audio, List<CarritoListModel> listacarrito, string totalAPagar)
        {
            InitializeComponent();
            SelectPagoOrden();
            this.fechaPedido = fechaPedido;
            this.fechaEntrega = fechaEntrega;
            this.idUbicacion = idUbicacion;
            this.audio = audio;
            this.listacarrito = listacarrito;
            this.totalAPagar = totalAPagar;
            cantidadefectivo_input.Text = totalAPagar;
            lblcantdebitar.Text = totalAPagar;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetTarjetasList();
        }

        string fechaPedido, fechaEntrega, idUbicacion, audio,totalAPagar;
        List<CarritoListModel> listacarrito;

        private void SelectPagoOrden()
        {
            radioefectivo.CheckedChanged += Radioefectivo_CheckedChanged;
            radiotarjeta.CheckedChanged += Radiotarjeta_CheckedChanged;
        }

        private void Radioefectivo_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radiobutton = sender as RadioButton;

            if (radiobutton.IsChecked)
            {
                layoutefectivo.IsVisible = true;
            }
            else if (!radiobutton.IsChecked)
            {
                layoutefectivo.IsVisible = false;
            }
        }

        private void Radiotarjeta_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radiobutton = sender as RadioButton;

            if (radiobutton.IsChecked)
            {
                layouttarjeta.IsVisible = true;
            } 
            else if (!radiobutton.IsChecked)
            {
                layouttarjeta.IsVisible = false;
            }
        }

        private void selecttarjeta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker pickerTarjeta = sender as Picker;
                var selectedItem = pickerTarjeta.SelectedItem;
                var itemValue = selectedItem.ToString().Split('-').First();                
                tarjetaId = itemValue;
            }
            catch (OperationCanceledException ocEx)
            {

            }
            catch (Exception ex)
            {

            }
        }

        private async void btnrealizarpagoefectivo_Clicked(object sender, EventArgs e)
        {
            try
            {
                var listDetalle = new List<OrdenDetalleModel>();

                foreach (var v in listacarrito)
                {
                    listDetalle.Add(new OrdenDetalleModel()
                    {
                        ID_Producto = v.ID_Producto.ToString(),
                        Cantidad = v.Cantidad.ToString(),
                        Precio = v.Precio.ToString()
                    });
                }

                SaveOrdenModel jsonObject = new SaveOrdenModel();
                jsonObject.Action = "add";
                jsonObject.Pedidos = new List<OrdenEncabezadoModel>();

                jsonObject.Pedidos.Add(new OrdenEncabezadoModel()
                {

                    ID_Cliente = correo,
                    ID_TipoPago = "1",
                    ID_TipoEntrega = "2",
                    FH_Pedido = fechaPedido,
                    FH_Entrega = fechaEntrega,
                    ID_Ubicacion = idUbicacion,
                    Comentarios = comentariopedidoefectivo_input.Text,
                    Audio = audio,
                    Estado = "1",
                    ID_Entregador = "",
                    DetallePedido = listDetalle

                });


                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(jsonObject);

                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["msg"].ToString();
                    await DisplayAlert("Success", "Orden registrada", "Ok");
                    await Navigation.PopAsync();

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

        private async void btnrealizarpagotarjeta_Clicked(object sender, EventArgs e)
        {
            try
            {
                var listDetalle = new List<OrdenDetalleModel>();

                foreach (var v in listacarrito)
                {
                    listDetalle.Add(new OrdenDetalleModel()
                    {
                        ID_Producto = v.ID_Producto.ToString(),
                        Cantidad = v.Cantidad.ToString(),
                        Precio = v.Precio.ToString()
                    });
                }

                SaveOrdenModel jsonObject = new SaveOrdenModel();
                jsonObject.Action = "add";
                jsonObject.Pedidos = new List<OrdenEncabezadoModel>();

                jsonObject.Pedidos.Add(new OrdenEncabezadoModel()
                {

                    ID_Cliente = correo,
                    ID_TipoPago = tarjetaId,
                    ID_TipoEntrega = "2",
                    FH_Pedido = fechaPedido,
                    FH_Entrega = fechaEntrega,
                    ID_Ubicacion = idUbicacion,
                    Comentarios = comentariopedidotarjeta_input.Text,
                    Audio = audio,
                    Estado = "1",
                    ID_Entregador = "",
                    DetallePedido = listDetalle

                });


                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(jsonObject);

                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["msg"].ToString();
                    await DisplayAlert("Success", "Orden registrada", "Ok");
                    await Navigation.PopAsync();
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

        private async void btnagregartarjetaorden_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaTarjetasPage());
        }

        private async void GetTarjetasList()
        {
            var AccesoInternet = Connectivity.NetworkAccess;

            if (AccesoInternet == NetworkAccess.Internet)
            {
                List<MetodosPagoListModel> listatarjetas = new List<MetodosPagoListModel>();
                listatarjetas = await ProductsApiController.ControllerObtenerListaMetodosPagos(correo);

                if (listatarjetas.Count > 0)
                {
                    selecttarjeta.ItemsSource = null;
                    //ls_tarjetas.ItemsSource = listaubicaciones;

                    var pickerList = new List<String>();

                    foreach (var v in listatarjetas)
                    {
                        pickerList.Add(v.ID_FormaPago.ToString() + "-" + v.NumTcTd.ToString() + " " + v.Titular.ToString());
                    }

                    selecttarjeta.ItemsSource = pickerList;
                    selecttarjeta.SelectedIndex = 0;

                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                }
            }
        }

    }
}