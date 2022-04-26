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
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MargaritasAppClase.Views.TabbedMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        public MenuPage()
        {
            InitializeComponent();
            //GetProductsList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetProductsList();
        }

        private async void btnagregarcarrito_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (sender as Button).BindingContext as ProductsListModel;
                //await DisplayAlert("Aviso", item.Descripcion.ToString(), "Ok");

                var listDetalle = new List<CarritoDetalleModel>();
                listDetalle.Add(new CarritoDetalleModel()
                {
                    ID_Producto = item.Id.ToString(),
                    Cantidad = "1",
                    Precio = item.Precio.ToString(),
                    Total = item.Precio.ToString()
                });

                SaveCarritoModel jsonObject = new SaveCarritoModel();
                jsonObject.Carrito = new List<CarritoEncabezadoModel>();

                double ldImpueto = 0.00, ldTotal = 0.00;
                ldImpueto = Convert.ToDouble(item.Precio.ToString()) * .15;
                ldTotal = Convert.ToDouble(item.Precio.ToString()) + ldImpueto;
                string totImpueto = "", totCarrito = "";
                totImpueto = ldImpueto.ToString("0.##");
                totCarrito = ldTotal.ToString("0.##");

                jsonObject.Carrito.Add(new CarritoEncabezadoModel()
                {

                    ID_Cliente = correo,
                    Fecha = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                    SubTotal = item.Precio.ToString(),
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
                    await DisplayAlert("Success", "Item: " + item.Descripcion.ToString() + ", agregado al carrito", "Ok");

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

        private async void GetProductsList()
        {
            try
            {
                var AccesoInternet = Connectivity.NetworkAccess;

                if (AccesoInternet == NetworkAccess.Internet)
                {
                    sl.IsVisible = true;
                    spinner.IsRunning = true;

                    List<ProductsListModel> listaproducros = new List<ProductsListModel>();
                    listaproducros = await ProductsApiController.ControllerObtenerListaProductos();

                    if (listaproducros.Count > 0)
                    {
                        listview_mainproductos.ItemsSource = null;
                        listview_mainproductos.ItemsSource = listaproducros;
                    }
                    else
                    {
                        await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                    }

                    sl.IsVisible = false;
                    spinner.IsRunning = false;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void listview_mainproductos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //ProductsListModel productsListModel = (ProductsListModel)e.Item;
            //await DisplayAlert("Aviso", productsListModel.Descripcion.ToString(), "Ok");
        }
    }
}