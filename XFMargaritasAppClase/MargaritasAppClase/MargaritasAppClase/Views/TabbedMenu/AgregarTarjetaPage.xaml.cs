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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views.TabbedMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarTarjetaPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        public AgregarTarjetaPage()
        {
            InitializeComponent();
        }

        private async void btnguardartarjeta_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(numtctd_input.Text) && String.IsNullOrEmpty(fechaexp_input.Text) && String.IsNullOrEmpty(cvv_input.Text) && String.IsNullOrEmpty(titular_input.Text))
                {
                    await DisplayAlert("Campo Vacio", "Por favor, Complete los campos requeridos ", "Ok");
                }
                else
                {

                    SaveMetodoPagoModel save = new SaveMetodoPagoModel
                    {
                        ID_Cliente = correo,
                        ID_FormaPago = "3",
                        NumTcTd = numtctd_input.Text,
                        FechaExp = fechaexp_input.Text,
                        Titular = titular_input.Text,
                        CCV = cvv_input.Text,
                        Predeterminada = "0",
                        Action = "add"
                    };

                    Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/payment/");

                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(save);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        String jsonx = response.Content.ReadAsStringAsync().Result;
                        JObject jsons = JObject.Parse(jsonx);
                        String Mensaje = jsons["msg"].ToString();

                        await DisplayAlert("Success", "Datos guardados correctamente", "Ok");

                        numtctd_input.Text = "";
                        fechaexp_input.Text = "";
                        titular_input.Text = "";
                        cvv_input.Text = "";
                        numtctd_input.Focus();
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
    }
}