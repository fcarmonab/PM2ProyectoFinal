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
    public partial class NewPassPage : ContentPage
    {
        public NewPassPage()
        {
            InitializeComponent();
        }

        private async void btnactualizarpassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.LoginPage());
        }
    }
}