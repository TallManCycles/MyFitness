using MyFitness.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class Welcome : ContentPage
    {
        public Welcome()
        {
            Color backgroundColor = Color.FromHex(Settings.BackgroundColor);
            Color fontColor = Color.FromHex(Settings.FontColor);

            InitializeComponent();
            this.BackgroundColor = backgroundColor;
            MainLabel.TextColor = fontColor;
            WelcomeButton.BackgroundColor = fontColor;
            WelcomeButton.TextColor = backgroundColor;
        }
        public void NextPage(object sender, EventArgs e)
        {
            MessagingCenter.Send<ContentPage>(this, "Swimming");
        }
    }
}
