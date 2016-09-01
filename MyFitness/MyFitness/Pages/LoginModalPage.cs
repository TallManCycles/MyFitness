using MyFitness.Helpers;
using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public class LoginModalPage : CarouselPage
    {
        ContentPage login;
        private ILoginManager _loginManager;

        public LoginModalPage(ILoginManager loginManager)
        {
            _loginManager = loginManager;

            login = new LoginPage();

            this.Children.Add(login);

            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) =>
            {
                this.SelectedItem = login;
            });
        }
    }
}
