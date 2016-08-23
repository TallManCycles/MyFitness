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
        ContentPage login, setup;
        private ILoginManager _loginManager;

        public LoginModalPage(ILoginManager loginManager)
        {
            _loginManager = loginManager;

            login = new LoginPage();

            setup = new InitialSetup(_loginManager);

            this.Children.Add(login);
            this.Children.Add(setup);

            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) =>
            {
                this.SelectedItem = login;
            });

            MessagingCenter.Subscribe<ContentPage>(this, "Setup", (sender) =>
            {
                this.SelectedItem = setup;
            });
        }
    }
}
