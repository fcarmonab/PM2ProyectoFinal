using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using System.Diagnostics;
using static Xamarin.Essentials.Permissions;
using Xamarin.Essentials;
using MargaritasAppClase.Controller;

namespace MargaritasAppClase.Views.EntregadorMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapaEntregadorPage : ContentPage
    {
        double lat, lon;
        public MapaEntregadorPage(double lat, double lon)
        {            
            InitializeComponent();
            this.lat = lat;
            this.lon = lon;
        }

        protected async override void OnAppearing() 
        {
            base.OnAppearing();
            irMapaEntregador();
        }

        private void Localizacion_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {

       /*     double Latituda = Convert.ToDouble("15.88555");
            double Longituda = Convert.ToDouble("-88.025441");
            var mapac = new Position(Latituda, Longituda);
            MapaEntregador.MoveToRegion(new MapSpan(mapac, 2, 2));*/

        }

        private async void btnMejorRuta_Clicked(object sender, EventArgs e)
        {
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            var location = new Location(lat, lon);
            await Xamarin.Essentials.Map.OpenAsync(location, options);
        }

        private async void irMapaEntregador() 
        {
            

            Pin pin = new Pin
            {
                Label = "Ubicacion del pedido",
                Type = PinType.Place,
                Position = new Position(lat, lon)
            };

            MapaEntregador.Pins.Add(pin);

            var location = await Geolocation.GetLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLastKnownLocationAsync();
            }

            MapaEntregador.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lon), Distance.FromMeters(2000)));

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
                MapaEntregador.MoveToRegion(MapSpan.FromCenterAndRadius(mapac, Distance.FromMeters(2000)));

            }
            else
            {
                await localizacion.GetLastKnownLocationAsync();
                var posicion = await localizacion.GetPositionAsync();
                var mapac = new Position(lat, lon);
                MapaEntregador.MoveToRegion(new MapSpan(mapac, 2, 2));
            }
        }


    }
}