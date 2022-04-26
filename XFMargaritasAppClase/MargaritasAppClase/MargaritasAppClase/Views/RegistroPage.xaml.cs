using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
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
using Plugin.LocalNotification;
using System.Net.Mail;
using System.Net.Mime;
namespace MargaritasAppClase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistroPage : ContentPage
    {
        public string AudioPath, fileName;
        public RegistroPage()
        {
            InitializeComponent();
            TapGestureRecognizer_Tapped();
        }

        byte[] imageToSave, audioToSave;

        private async void btntomarphoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                var takepic = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "PhotoApp",
                    Name = DateTime.Now.ToString() + "_Pic.jpg",
                    SaveToAlbum = true,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                    CompressionQuality = 40
                });

              //  await DisplayAlert("Ubicacion de la foto: ", takepic.Path, "Ok");

                if (takepic != null)
                {
                    imageToSave = null;
                    MemoryStream memoryStream = new MemoryStream();

                    takepic.GetStream().CopyTo(memoryStream);
                    imageToSave = memoryStream.ToArray();

                    registroimg.Source = ImageSource.FromStream(() => { return takepic.GetStream(); });
                }
                
                //descripcion_entry.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async void btnregistrarusuario_Clicked(object sender, EventArgs e)
        {

            try
            {
                if (String.IsNullOrEmpty(nombreregistro_input.Text) && String.IsNullOrEmpty(apellidoregistro_input.Text) && String.IsNullOrEmpty(correoregistro_input.Text) && String.IsNullOrEmpty(telefonoregistro_input.Text) && String.IsNullOrEmpty(password_input.Text) && String.IsNullOrEmpty(confirmarpassword_input.Text))
                {
                    await DisplayAlert("Campo Vacio", "Por favor, Complete los campos requeridos ", "Ok");
                }
                else
                {
                    //convertir la imagen a base64
                    string pathBase64Imagen = Convert.ToBase64String(imageToSave);
                    
                    //extraer el path del audio
                    //string audio = AudioPath;
                    //convertir a arreglo de bytes
                    //byte[] fileByte = System.IO.File.ReadAllBytes(audio);
                    //convertir el audio a base64
                    //string pathBase64Audio = Convert.ToBase64String(fileByte);

                    ClientesModel save = new ClientesModel
                    {
                        ID_Cliente = "0",
                        Nombre = nombreregistro_input.Text,
                        Apellido = apellidoregistro_input.Text,
                        Correo = correoregistro_input.Text,
                        Contrasena = password_input.Text,
                        FechaNac = "0",
                        FechaCrea = "0",
                        Telefono = telefonoregistro_input.Text,
                        Foto = pathBase64Imagen,
                        Estado = "0",
                        TipoUsuario = "1",
                    };

                    Uri RequestUri = new Uri("https://webfacturacesar.000webhostapp.com/Margarita/methods/cliente/add.php");

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


                        //Codigo para enviar email de confirmacion de registro

                        try
                        {

                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                            mail.From = new MailAddress("notificadormargaritaapp@gmail.com");
                            mail.To.Add(correoregistro_input.Text);
                            mail.Subject = "Confirmación de Registro Margarita App";
                            //mail.Body = txtBody.Text;

                            string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                            body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";

                            // Se puede insertar imagen al body del correo si asi se desea.
                            body += "</HEAD><BODY><DIV><IMG src='https://us.123rf.com/450wm/tuktukpranee/tuktukpranee1905/tuktukpranee190500023/127432966-suave-desenfoque-de-fondo-hermosa-cubierta-de-flor-de-flor-de-cerezo-rosa-sakura-con-nieve-en-plena-.jpg?ver=6' width='300' height='100'><br><br>"; 
                            
                            //enlace url para cambiar codigo
                            body += $"<a href='https://webfacturacesar.000webhostapp.com/Margarita/methods/cliente/verification.php?mail={correoregistro_input.Text}' Color='#FC78D8'>Confirma tus credenciales haciendo clic aqui!</a>";
                            body += "</DIV></BODY></HTML>";

                            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                            mail.AlternateViews.Add(alternate);
                            SmtpServer.Port = 587;
                            SmtpServer.Host = "smtp.gmail.com";
                            SmtpServer.EnableSsl = true;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("notificadormargarita2@gmail.com", "AppMargarita");

                            SmtpServer.Send(mail);

                            //envia Push Notificacion
                            var notificacion = new NotificationRequest
                            {
                                BadgeNumber = 1,
                                Title = "Bienvenido a MargaritaApp",
                                ReturningData = "Dummy Data",
                                NotificationId = 1337,

                            };

                            await NotificationCenter.Current.Show(notificacion);
                            //Finaliza el Push de Notificacion
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Faild", ex.Message, "OK");
                        }

                        nombreregistro_input.Text = "";
                        apellidoregistro_input.Text = "";
                        correoregistro_input.Text = "";
                        telefonoregistro_input.Text = "";
                        password_input.Text = "";
                        confirmarpassword_input.Text = "";
                        imageToSave = null;
                        registroimg.Source = null;
                        nombreregistro_input.Focus();

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

            //await Navigation.PushAsync(new Views.ConfirmarUsuarioPage());

        }

        private void TapGestureRecognizer_Tapped()
        {
            lbl_loginpage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new Views.LoginPage());
                })
            });
        }
    }
}
