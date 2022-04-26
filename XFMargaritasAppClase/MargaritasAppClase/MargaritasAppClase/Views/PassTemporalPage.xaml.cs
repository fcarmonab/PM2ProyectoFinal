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
    public partial class CorreoCodePage : ContentPage
    {
        public CorreoCodePage()
        {
            InitializeComponent();
        }

        private async void btnconfirmarcl_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPassPage());
        }
    }
}