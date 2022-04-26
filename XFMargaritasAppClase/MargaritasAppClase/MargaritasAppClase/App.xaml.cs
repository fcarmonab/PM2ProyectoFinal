﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MargaritasAppClase
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new Views.TabbedMenu.MainTabbedPage());
            MainPage = new NavigationPage(new Views.LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
