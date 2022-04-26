using MargaritasAppClase.Controller;
using MargaritasAppClase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//using Android.Media;
//using Stream = Android.Media.Stream;
//using Android.Util;

namespace MargaritasAppClase.Views.TabbedMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetallePedidoPage : ContentPage
    {
        string correo = Application.Current.Properties["correo"].ToString();
        string correlativo,fecha,direccion,audio;
        byte[] decodedString = null;
        public DetallePedidoPage(string correlativo, string fecha, string direccion, string audio)
        {
            this.correlativo = correlativo;
            this.fecha = fecha;
            this.direccion = direccion;
            this.audio = audio;

            InitializeComponent();
            GetOrdenDetalleList();
        }

        private async void btnescucharaudioreferencia_Clicked(object sender, EventArgs e)
        {
            /*
            if(audio.Length > 0)
            {
                decodedString = Base64.Decode(audio, Base64Flags.Default);
            }

            if (decodedString != null)
            {
                try
                {
                    PlayAudioTrack(decodedString);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                await DisplayAlert("Notificación", $"Orden sin audio de referencia", "Ok");
            }
            */
        }


        private async void GetOrdenDetalleList()
        {
            var AccesoInternet = Connectivity.NetworkAccess;

            if (AccesoInternet == NetworkAccess.Internet)
            {
                sl_detallepedido.IsVisible = true;
                spinner_detallepedido.IsRunning = true;

                List<ClienteListaPedidosDetalleModel> listaordendetalle = new List<ClienteListaPedidosDetalleModel>();
                listaordendetalle = await ProductsApiController.ControllerObtenerListaOrdenesClienteDetalle(correo,correlativo);

                listview_detallepedido.ItemsSource = null;
                double subtotal = 0, impuesto = 0, total = 0;

                if (listaordendetalle.Count > 0)
                {
                    //listview_detallepedido.ItemsSource = null;
                    listview_detallepedido.ItemsSource = listaordendetalle;

                    foreach (var v in listaordendetalle)
                    {
                        subtotal = subtotal + Convert.ToDouble(v.Cantidad.ToString()) * Convert.ToDouble(v.Precio.ToString());
                    }
                    impuesto = subtotal * .15;
                    total = subtotal + impuesto;
                }
                else
                {
                    await DisplayAlert("Notificación", $"Lista vacía, ingrese datos", "Ok");
                }

                int panio, pmes, pdia, phora, pminuto, psegundo;
                DateTime fechaPedido = Convert.ToDateTime(fecha);
                panio = Convert.ToInt32(fechaPedido.ToString("yyyy"));
                pmes = Convert.ToInt32(fechaPedido.ToString("MM"));
                pdia = Convert.ToInt32(fechaPedido.ToString("dd"));
                phora = Convert.ToInt32(fechaPedido.ToString("HH"));
                pminuto = Convert.ToInt32(fechaPedido.ToString("mm"));
                psegundo = Convert.ToInt32(fechaPedido.ToString("ss"));

                dateverfechadeentrega.Date = new DateTime(panio, pmes, pdia);
                timeverhoraentrega.Time = new TimeSpan(phora,pminuto,psegundo);

                lblUbicacion.Text = direccion;
                lblsubtotaldetalleorden.Text = "L. " + subtotal.ToString("#,#.00");
                lblisvdetalleorden.Text = "L. " + impuesto.ToString("#,#.00");
                lbltotalapagardetalleorden.Text = "L. " + total.ToString("#,#.00");

                sl_detallepedido.IsVisible = false;
                spinner_detallepedido.IsRunning = false;
            }

        }
        
        /*
        void PlayAudioTrack(byte[] audioBuffer)
        {
            AudioTrack audioTrack = new AudioTrack(
              // Stream type
              Stream.Music,
              // Frequency
              24000,
              // Mono or stereo
              ChannelOut.Stereo,
              // Audio encoding
              Android.Media.Encoding.Pcm16bit,
              // Length of the audio clip.
              audioBuffer.Length,
              // Mode. Stream or static.
              AudioTrackMode.Stream);

            audioTrack.Play();
            audioTrack.Write(audioBuffer, 0, audioBuffer.Length);
        }
        */
    }
}