using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PortraitPage : ContentPage
    {
        public PortraitPage()
        {
            InitializeComponent();
        }

        private async void btngotologin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.LoginPage());
        }

        private async void btngotoregistro_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.RegistroPage());
        }
    }
}