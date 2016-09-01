using System;
using System.Runtime.CompilerServices;
using MyFitness.Helpers;
using Xamarin.Forms;

namespace MyFitness.Pages
{
    public class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.BackgroundColor = Color.FromHex(Settings.BackgroundColor);

            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) =>
            {
                this.SendBackButtonPressed();
            });
        }
    }
}