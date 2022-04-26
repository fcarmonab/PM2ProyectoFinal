using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MargaritasAppClase.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using MargaritasAppClase.Views;
using System.IO;
using Plugin.LocalNotification;

namespace MargaritasAppClase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string pdCorreo = "", tipoUsuario = "";        
        public LoginPage()
        {
            InitializeComponent();
            TapRegistroPage_Tapped();
            TapForgotPassPage_Tapped();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.Properties.ContainsKey("correo"))
            {
                if (Application.Current.Properties.ContainsKey("tipousuario"))
                {
                    string pdTipoUsuario = Application.Current.Properties["tipousuario"].ToString();
                    if (pdTipoUsuario == "2")
                    {
                        await Navigation.PushAsync(new Views.EntregadorMenu.MenuEntregadorTabbedPage());
                        Navigation.RemovePage(Navigation.NavigationStack[0]);
                        //int numPage = Navigation.NavigationStack.Count;
                        //await DisplayAlert("Mensaje", numPage.ToString(), "Ok");
                    }
                    else
                    {
                        await Navigation.PushAsync(new Views.TabbedMenu.MainTabbedPage());
                        Navigation.RemovePage(Navigation.NavigationStack[0]);
                        //int numPage = Navigation.NavigationStack.Count;
                        //await DisplayAlert("Mensaje", numPage.ToString(), "Ok");
                    }
                }
            }
        }

        private async void btniniciarsesion_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(correo_input.Text) && String.IsNullOrEmpty(password_input.Text))
            {
                await DisplayAlert("Campo Vacio", "Por favor, Ingrese un correo y una contraseña ", "Ok");
            }
            else
            {

                LoginModel Login = new LoginModel
                {
                    authmail = correo_input.Text,
                    authpass = password_input.Text,
                };

                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/login/");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(Login);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["success"].ToString();
                    String mensajeTipoUsuario = jsons["TipoUsuario"].ToString();

                    //await DisplayAlert("Success", "Datos guardados correctamente", "Ok");


                    if (Mensaje == "true")
                    {

                        pdCorreo = correo_input.Text;
                        tipoUsuario = mensajeTipoUsuario;

                        Application.Current.Properties["correo"] = pdCorreo;
                        Application.Current.Properties["tipousuario"] = tipoUsuario;

                        await Application.Current.SavePropertiesAsync();

                        if (mensajeTipoUsuario == "2")
                        {

                            await Navigation.PushAsync(new Views.EntregadorMenu.MenuEntregadorTabbedPage());
                         
                            var notificacion = new NotificationRequest
                            {
                                BadgeNumber = 1,
                                Title = "Pedidos Pendientes",
                                Description = "Por favor, dar seguimiento a sus pedidos",
                                ReturningData = "Dummy Data",
                                NotificationId = 1337,

                            };

                            await NotificationCenter.Current.Show(notificacion);
                        }
                        else
                        {
                            await Navigation.PushAsync(new Views.TabbedMenu.MainTabbedPage());

                            //envia Push Notificacion
                            var notificacion = new NotificationRequest
                            {
                                BadgeNumber = 1,
                                Title = "Te extramos",
                                Description = "Tenemos muchas promociones para ti",
                                ReturningData = "Dummy Data",
                                NotificationId = 1337,

                            };

                            await NotificationCenter.Current.Show(notificacion);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "El usuario no existe", "Ok");
                        correo_input.Text = "";
                        password_input.Text = "";
                        correo_input.Focus();
                    }

                    //await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                }
            }
        }

        private void TapRegistroPage_Tapped() => lbl_registropage.GestureRecognizers.Add(new TapGestureRecognizer()
        {
            Command = new Command(() =>
            {
                Navigation.PushAsync(new Views.RegistroPage());
            })
        });

        private void TapForgotPassPage_Tapped() => lbl_forgotpasspage.GestureRecognizers.Add(new TapGestureRecognizer()
        {
            Command = new Command(() =>
            {
                Navigation.PushAsync(new Views.ForgotPassPage());
            })
        });

    }

}

