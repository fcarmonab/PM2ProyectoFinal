using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using MargaritasAppClase.Models;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotification;

namespace MargaritasAppClase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassPage : ContentPage
    {
        string pdCorreo = "";
        public ForgotPassPage()
        {
            InitializeComponent();
        }

        private async void btnenviarcorreo_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new CorreoCodePage());
            if (String.IsNullOrEmpty(correofp_input.Text))
            {
                await DisplayAlert("Campo Vacio", "Por favor, Ingrese un correo valido", "Ok");
            }
            else
            {

                ForgotPassModel mail = new ForgotPassModel
                {
                    mail = correofp_input.Text
                };
                
                    

                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/recoveryPass/");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(mail);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["success"].ToString();
                    //String mensajemail = jsons["mail"].ToString();

                    //await DisplayAlert("Success", "Datos guardados correctamente", "Ok");


                    if (Mensaje == "true")
                    {

                        pdCorreo = correofp_input.Text;
                        //tipoUsuario = mensajeTipoUsuario;

                        Application.Current.Properties["correo"] = pdCorreo;
                        //Application.Current.Properties["tipousuario"] = tipoUsuario;

                        await Application.Current.SavePropertiesAsync();
                        
                        await DisplayAlert("Mensaje", "El codigo de recuperacion fue enviado a tu correo", "Ok");
                        //envia Push Notificacion
                        
                        Application.Current.Properties.Remove("correo");
                        Application.Current.Properties.Remove("tipousuario");

                        await Navigation.PushAsync(new Views.LoginPage());

                    }
                    else
                    {
                        await DisplayAlert("Error", "El Correo no existe", "Ok");
                        correofp_input.Text = "";
                        
                    }

                    //await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                }
            }
        }
    }
}