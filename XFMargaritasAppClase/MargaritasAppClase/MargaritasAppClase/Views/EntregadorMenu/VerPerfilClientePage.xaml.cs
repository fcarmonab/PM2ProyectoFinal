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
    public partial class VerPerfilClientePage : ContentPage
    {

        byte[] newBytes = null;
        string id = "", nombre = "", apellido = "", telefono = "", foto = "";
        string correoClientePedido;

        public VerPerfilClientePage(string correoClientePedido)
        {
            this.correoClientePedido = correoClientePedido;
            InitializeComponent();
            GetPerfilForId();

        }

        private async void GetPerfilForId()
        {
            GetPerfilModel getPerfil = new GetPerfilModel
            {
                authmail = correoClientePedido,
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

                lblnombrecliente.Text = nombre;
                lblapellidocliente.Text = apellido;
                lbltelefonocliente.Text = telefono;
                lblcorreocliente.Text = correoClientePedido;
                imgclienteperfil.Source = ImageSource.FromStream(() => stream);
            }
            else
            {
                await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
            }
        }
    }
}