using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public class Setup : CarouselPage
    {
        ContentPage swimming, running, welcome, instructions;
        private ILoginManager _loginManager;

        public Setup(ILoginManager loginManager)
        {
            _loginManager = loginManager;

            welcome = new Welcome();

            swimming = new Swimming();

            running = new Running(loginManager);

            //instructions = new Instructions();

            this.Children.Add(welcome);
            this.Children.Add(swimming);
            this.Children.Add(running);
            //this.Children.Add(instructions);

            MessagingCenter.Subscribe<ContentPage>(swimming, "Swimming", (sender) =>
            {
                this.SelectedItem = swimming;
            });

            MessagingCenter.Subscribe<ContentPage>(running, "Running", (sender) =>
            {
                this.SelectedItem = running;
            });
        }
    }
}
