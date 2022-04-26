using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views.TabbedMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapaSeguirPedidoPage : ContentPage
    {
        double lat, lon, latped, lonped;
        public MapaSeguirPedidoPage(double lat, double lon, double latped, double lonped)
        {
            this.lat = lat;
            this.lon = lon;
            this.latped = latped;
            this.lonped = lonped;
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            irMapaSeguirPedido();
        }

        private void Localizacion_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
                  var mapac = new Position(latped, lonped);
                  mpseguirpedido.MoveToRegion(new MapSpan(mapac, 2, 2));
        }


        private async void irMapaSeguirPedido()
        {
            Pin pin = new Pin
            {
                Label = "Destino pedido",
                Type = PinType.SavedPin,                
                Position = new Position(lat, lon)
            };

            Pin pin2 = new Pin
            {
                Label = "Ubicacion Actual del pedido",
                Type = PinType.Place,
                Position = new Position(latped, lonped),
                
            };

            mpseguirpedido.Pins.Add(pin);
            mpseguirpedido.Pins.Add(pin2);

            var location = await Geolocation.GetLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLastKnownLocationAsync();
            }

            mpseguirpedido.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lon), Distance.FromMeters(2000)));

            var localizacion = CrossGeolocator.Current;

            if (localizacion != null)
            {
                localizacion.PositionChanged += Localizacion_PositionChanged;

                if (!localizacion.IsListening)
                {
                    Debug.WriteLine("StartListeningAsync");
                    await localizacion.StartListeningAsync(TimeSpan.FromMinutes(5), 100);
                }

                var posicion = await localizacion.GetPositionAsync();
                var mapac = new Position(lat, lon);
                mpseguirpedido.MoveToRegion(MapSpan.FromCenterAndRadius(mapac, Distance.FromMeters(2000)));

            }
            else
            {
                await localizacion.GetLastKnownLocationAsync();
                var posicion = await localizacion.GetPositionAsync();
                var mapac = new Position(lat, lon);
                mpseguirpedido.MoveToRegion(new MapSpan(mapac, 2, 2));
            }
        }
    }
}