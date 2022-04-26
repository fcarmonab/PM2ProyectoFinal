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
    public partial class HistorialEntregadosPage : ContentPage
    {

        List<EntregadorListPedidosModel> listaordenesentregadas = null;

        string correo = Application.Current.Properties["correo"].ToString();

        public HistorialEntregadosPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetOrdenesEntregadasList();
        }


        private async void GetOrdenesEntregadasList()
        {
            try
            {
                var AccesoInternet = Connectivity.NetworkAccess;

                if (AccesoInternet == NetworkAccess.Internet)
                {
                    sl_historialpedidoscompletadosentregador.IsVisible = true;
                    spinner_historialpedidoscompletadosentregador.IsRunning = true;

                    listaordenesentregadas = new List<EntregadorListPedidosModel>();
                    listaordenesentregadas = await ProductsApiController.ControllerObtenerListaOrdenesEntregadas(correo);

                    if (listaordenesentregadas.Count > 0)
                    {
                        listview_historialpedidoscompletadosentregador.ItemsSource = null;
                        listview_historialpedidoscompletadosentregador.ItemsSource = listaordenesentregadas;
                    }
                    else
                    {
                        await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                    }

                    sl_historialpedidoscompletadosentregador.IsVisible = false;
                    spinner_historialpedidoscompletadosentregador.IsRunning = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}