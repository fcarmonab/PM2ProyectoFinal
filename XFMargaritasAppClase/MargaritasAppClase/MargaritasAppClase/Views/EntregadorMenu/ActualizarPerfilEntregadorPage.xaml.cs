using MargaritasAppClase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views.EntregadorMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarPerfilEntregadorPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        byte[] imageToSave;
        public ActualizarPerfilEntregadorPage(byte[] newBytes)
        {
            this.imageToSave = newBytes;
            InitializeComponent();
        }

        private async void btntomarphotoactualizadarepartidor_Clicked(object sender, EventArgs e)
        {
            try
            {
                var takepic = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "PhotoApp",
                    Name = DateTime.Now.ToString() + "_Pic.jpg",
                    SaveToAlbum = true,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    CompressionQuality = 40
                });

                //  await DisplayAlert("Ubicacion de la foto: ", takepic.Path, "Ok");

                if (takepic != null)
                {
                    imageToSave = null;
                    MemoryStream memoryStream = new MemoryStream();

                    takepic.GetStream().CopyTo(memoryStream);
                    imageToSave = memoryStream.ToArray();

                    imgeditperfilrepartidor.Source = ImageSource.FromStream(() => { return takepic.GetStream(); });
                }

                //descripcion_entry.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void btnguardarcambiosrepartidor_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(nombreactualizarrepartidor_input.Text) && String.IsNullOrEmpty(apellidoactualizarrepartidor_input.Text) && String.IsNullOrEmpty(telefonoactualizarrepartidor_input.Text))
                {
                    await DisplayAlert("Campo Vacio", "Por favor, Complete los campos requeridos ", "Ok");
                }
                else
                {
                    //convertir la imagen a base64
                    string pathBase64Imagen = Convert.ToBase64String(imageToSave);


                    ActualizarClienteModel save = new ActualizarClienteModel
                    {
                        Nombre = nombreactualizarrepartidor_input.Text,
                        Apellido = apellidoactualizarrepartidor_input.Text,
                        Correo = correo,
                        Contrasena = "1234",
                        Telefono = telefonoactualizarrepartidor_input.Text,
                        Foto = pathBase64Imagen,
                    };

                    Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/cliente/upd.php");

                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(save);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(RequestUri, contentJson);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        String jsonx = response.Content.ReadAsStringAsync().Result;
                        JObject jsons = JObject.Parse(jsonx);
                        String Mensaje = jsons["msg"].ToString();

                        await DisplayAlert("Success", "Perfil actualizado correctamente", "Ok");

                        //Codigo para enviar email de confirmacion de registro
                        try
                        {

                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                            mail.From = new MailAddress("notificadormargaritaapp@gmail.com");
                            mail.To.Add(correo);
                            mail.Subject = "Actualizacion de perfil Margarita App";
                            //mail.Body = txtBody.Text;

                            string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                            body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";

                            // Se puede insertar imagen al body del correo si asi se desea.
                            body += "</HEAD><BODY><DIV><IMG src='https://us.123rf.com/450wm/tuktukpranee/tuktukpranee1905/tuktukpranee190500023/127432966-suave-desenfoque-de-fondo-hermosa-cubierta-de-flor-de-flor-de-cerezo-rosa-sakura-con-nieve-en-plena-.jpg?ver=6' width='300' height='100'><br><br>";

                            //enlace url para cambiar codigo
                            body += "<a href='https://www.uth.hn' Color='#FC78D8'>Confirma tus credenciales haciendo clic aqui!</a>";
                            body += "</DIV></BODY></HTML>";

                            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                            mail.AlternateViews.Add(alternate);
                            SmtpServer.Port = 587;
                            SmtpServer.Host = "smtp.gmail.com";
                            SmtpServer.EnableSsl = true;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("notificadormargaritaapp@gmail.com", "AppMargarita");

                            SmtpServer.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            DisplayAlert("Faild", ex.Message, "OK");
                        }

                        nombreactualizarrepartidor_input.Text = "";
                        apellidoactualizarrepartidor_input.Text = "";

                        telefonoactualizarrepartidor_input.Text = "";
                        imageToSave = null;
                        imgeditperfilrepartidor.Source = null;
                        nombreactualizarrepartidor_input.Focus();

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

          
            await Navigation.PushAsync(new Views.EntregadorMenu.MenuEntregadorPage());
            Navigation.RemovePage(Navigation.NavigationStack[0]);

        }
    }
}