using MargaritasAppClase.Controller;
using MargaritasAppClase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views.EntregadorMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntregadorDetallePedidoPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        string correlativo, fecha, direccion, correoCliente, audio;
        byte[] decodedString = null;

        public EntregadorDetallePedidoPage(string correlativo, string fecha, string direccion, string correoCliente, string audio)
        {
            this.correlativo = correlativo;
            this.fecha = fecha;
            this.direccion = direccion;
            this.correoCliente = correoCliente;
            this.audio = audio;

            InitializeComponent();
            GetOrdenDetalleList();
        }

        private async void GetOrdenDetalleList()
        {
            var AccesoInternet = Connectivity.NetworkAccess;

            if (AccesoInternet == NetworkAccess.Internet)
            {
                sl_detallepedidoentregador.IsVisible = true;
                spinner_detallepedidoentregador.IsRunning = true;

                List<ClienteListaPedidosDetalleModel> listaordendetalle = new List<ClienteListaPedidosDetalleModel>();
                listaordendetalle = await ProductsApiController.ControllerObtenerListaOrdenesClienteDetalle(correoCliente, correlativo);

                listview_detallepedidoentregador.ItemsSource = null;
                double subtotal = 0, impuesto = 0, total = 0;

                if (listaordendetalle.Count > 0)
                {
                    //listview_detallepedido.ItemsSource = null;
                    listview_detallepedidoentregador.ItemsSource = listaordendetalle;

                    foreach (var v in listaordendetalle)
                    {
                        subtotal = subtotal + Convert.ToDouble(v.Cantidad.ToString()) * Convert.ToDouble(v.Precio.ToString());
                    }
                    impuesto = subtotal * .15;
                    total = subtotal + impuesto;
                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                }

                int panio, pmes, pdia, phora, pminuto, psegundo;
                DateTime fechaPedido = Convert.ToDateTime(fecha);
                panio = Convert.ToInt32(fechaPedido.ToString("yyyy"));
                pmes = Convert.ToInt32(fechaPedido.ToString("MM"));
                pdia = Convert.ToInt32(fechaPedido.ToString("dd"));
                phora = Convert.ToInt32(fechaPedido.ToString("HH"));
                pminuto = Convert.ToInt32(fechaPedido.ToString("mm"));
                psegundo = Convert.ToInt32(fechaPedido.ToString("ss"));

                dateverfechadeentregaentregador.Date = new DateTime(panio, pmes, pdia);
                timeverhoraentregaentregador.Time = new TimeSpan(phora, pminuto, psegundo);

                lblUbicacionentregador.Text = direccion;
                lblsubtotaldetalleordenentregador.Text = "L. " + subtotal.ToString("#,#.00");
                lblisvdetalleordenentregador.Text = "L. " + impuesto.ToString("#,#.00");
                lbltotalapagardetalleordenentregador.Text = "L. " + total.ToString("#,#.00");

                sl_detallepedidoentregador.IsVisible = false;
                spinner_detallepedidoentregador.IsRunning = false;
            }

        }

        private void btnescucharaudioreferenciaentregador_Clicked(object sender, EventArgs e)
        {

        }
    }
}