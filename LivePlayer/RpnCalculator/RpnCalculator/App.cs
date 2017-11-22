﻿using System;

using Xamarin.Forms;

namespace RpnCalculator
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            var content = new ContentPage
            {
                Title = "RpnCalculator",

            };
            var calc = new Calculator();
            content.Content = calc.Make(content);
            MainPage = content;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
