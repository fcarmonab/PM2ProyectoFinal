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
    public partial class EvaluacionPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();

        string correl;
        int nota1 = 0, nota2 = 0, nota3 = 0, nota4 = 0;

        public EvaluacionPage(string correl)
        {
            InitializeComponent();
            this.correl = correl;
        }

        /* Pregunta 1 */
        private void rb_excelentepreguntauno_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota1 = 5;
        }

        private void rb_muybuenopreguntauno_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota1 = 4;
        }

        private void rb_buenopreguntauno_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota1 = 3;
        }

        private void rb_malopreguntauno_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota1 = 2;
        }

        private void rb_muymalopreguntauno_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota1 = 1;
        }

        /* Pregunta 2 */
        private void rb_excelentepreguntados_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota2 = 5;
        }

        private void rb_muybuenopreguntados_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota2 = 4;
        }

        private void rb_buenopreguntados_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota2 = 3;
        }

        private void rb_malopreguntados_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota2 = 2;
        }

        private void rb_muymalopreguntados_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota2 = 1;
        }

        /* Pregunta 3 */
        private void rb_excelentepreguntautres_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota3 = 5;
        }

        private void rb_muybuenopreguntatres_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota3 = 4;
        }

        private void rb_buenopreguntatres_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota3 = 3;
        }

        private void rb_malopreguntatres_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota3 = 2;
        }

        private void rb_muymalopreguntatres_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota3 = 1;
        }

        /* Pregunta 4 */
        private void rb_excelentepreguntacuatro_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota4 = 5;
        }

        private void rb_muybuenopreguntacuatro_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota4 = 4;
        }

        private void rb_buenopreguntacuatro_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota4 = 3;
        }

        private void rb_malopreguntacuatro_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota4 = 2;
        }

        private void rb_muymalopreguntacuatro_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            nota4 = 1;
        }

        private async void btnenviarevaluacion_Clicked(object sender, EventArgs e)
        {
            if (nota1 > 0 && nota2 > 0 && nota3 > 0 && nota4 > 0)
            {
                SaveEvaluacionModel save = new SaveEvaluacionModel
                {
                    Action = "cal",
                    ID_Cliente = correo,
                    Correl = correl,
                    Calif1 = nota1.ToString(),
                    Calif2 = nota2.ToString(),
                    Calif3 = nota3.ToString(),
                    Calif4 = nota4.ToString()
                };

                Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/orders/");

                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(save);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    String jsonx = response.Content.ReadAsStringAsync().Result;
                    JObject jsons = JObject.Parse(jsonx);
                    String Mensaje = jsons["msg"].ToString();
                    await DisplayAlert("Success", "Formulario Enviado", "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Estamos en mantenimiento", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Margarita App", "Debe responder todas las preguntas", "Ok");
            }
        }

    }
}