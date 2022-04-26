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
    public partial class ListaTarjetasPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        public ListaTarjetasPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetTarjetasList();
        }

        private async void tbiagregartarjetaspage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgregarTarjetaPage());
        }

        private void ls_tarjetas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void ls_tarjetas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                MetodosPagoListModel MetodosPagoListModel = (MetodosPagoListModel)e.Item;

                var messageAlert = await DisplayAlert("Opción", "Seleccione una opción", "Editar", "Eliminar");

                if (messageAlert)
                {
                    // editar ubicacion
                    var changeMetodoPagoBinding = new MetodosPagoModel
                    {
                        ID_Cliente = correo,
                        ID_FormaPago = MetodosPagoListModel.ID_FormaPago,
                        NumTcTd = MetodosPagoListModel.NumTcTd,
                        Fecha_exp = MetodosPagoListModel.Fecha_exp,
                        Titular = MetodosPagoListModel.Titular,
                        CCV = MetodosPagoListModel.CCV,
                        Predeterminada = MetodosPagoListModel.Predeterminada
                    };

                    /*
                    var openPageActualizarMetodoPago = new ActualizarTarjetasPage();
                    openPageActualizarMetodoPago.BindingContext = changeMetodoPagoBinding;

                    await Navigation.PushAsync(openPageActualizarMetodoPago);
                    */

                    await DisplayAlert("Mensaje", "No es permitido modificar formas de pago", "Ok");
                }
                else
                {
                    try
                    {
                        EliminarMetodoPagoModel save = new EliminarMetodoPagoModel
                        {
                            ID_Cliente = correo,
                            ID_Rel_Pago = MetodosPagoListModel.ID_Rel_Pago,
                            Action = "del"
                        };

                        Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/payment/ ");

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
                            GetTarjetasList();
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

        private async void GetTarjetasList()
        {
            var AccesoInternet = Connectivity.NetworkAccess;

            if (AccesoInternet == NetworkAccess.Internet)
            {
                sltarjetas.IsVisible = true;
                spinnertarjetas.IsRunning = true;

                List<MetodosPagoListModel> listaubicaciones = new List<MetodosPagoListModel>();
                listaubicaciones = await ProductsApiController.ControllerObtenerListaMetodosPagos(correo);

                if (listaubicaciones.Count > 0)
                {
                    ls_tarjetas.ItemsSource = null;
                    ls_tarjetas.ItemsSource = listaubicaciones;
                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                }

                sltarjetas.IsVisible = false;
                spinnertarjetas.IsRunning = false;
            }
        }
    }
}