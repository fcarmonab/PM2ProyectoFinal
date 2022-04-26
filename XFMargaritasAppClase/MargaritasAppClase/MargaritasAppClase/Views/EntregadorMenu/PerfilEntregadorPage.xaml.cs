using MargaritasAppClase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views.EntregadorMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilEntregadorPage : ContentPage
    {
        byte[] newBytes = null;
        string id = "", nombre = "", apellido = "", telefono = "", foto = "", correo = Application.Current.Properties["correo"].ToString();

        public PerfilEntregadorPage()
        {
            InitializeComponent();
            GetPerfilForId();
        }

        private async void btncerrarsesionrepartidor_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Remove("correo");
            await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(Navigation.NavigationStack[0]);
        }


        private async void btnactualizarperfilrepartidorpage_Clicked(object sender, EventArgs e)
        {
            var stream = new MemoryStream(newBytes);
            var changePerfilBinding = new PerfilEntregadorModel
            {
                ID_Cliente = id,
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                FechaNac = "",
                FechaCrea = "",
                Telefono = telefono,
                fotografia = ImageSource.FromStream(() => stream),
                Foto = "",
                Estado = "",
                TipoUsuario = "2",
            };

            var openPageActualizarPerfil = new ActualizarPerfilEntregadorPage(newBytes);
            openPageActualizarPerfil.BindingContext = changePerfilBinding;

            await Navigation.PushAsync(openPageActualizarPerfil);
        }

        private async void GetPerfilForId()
        {
            GetPerfilModel getPerfil = new GetPerfilModel
            {
                authmail = correo,
            };

            Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/cliente/");
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(getPerfil);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(RequestUri, contentJson);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                String jsonx = response.Content.ReadAsStringAsync().Result;
                JObject jsons = JObject.Parse(jsonx);

                string contenido = response.Content.ReadAsStringAsync().Result.ToString();

                dynamic dyn = JsonConvert.DeserializeObject(contenido);
                var stream = new MemoryStream();
                foreach (var item in dyn.items)
                {
                    string img64 = item.Foto.ToString();
                    newBytes = Convert.FromBase64String(img64);
                    stream = new MemoryStream(newBytes);

                    id = item.ID_Cliente.ToString();
                    nombre = item.Nombre.ToString();
                    apellido = item.Apellido.ToString();
                    telefono = item.Telefono.ToString();
                }

                lbNombreRepartidor.Text = nombre;
                imgperfilrepartidor.Source = ImageSource.FromStream(() => stream);
            }
            else
            {
                await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
            }
        }
    }
}