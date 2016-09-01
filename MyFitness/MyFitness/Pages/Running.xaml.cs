using MyFitness.Helpers;
using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class Running : ContentPage
    {
        private List<int> pickerDataSource;
        private ILoginManager _loginManager;

        public Running(ILoginManager loginManager)
        {
            InitializeComponent();

            _loginManager = loginManager;

            Color backgroundColor = Color.FromHex(Settings.BackgroundColor);
            Color fontColor = Color.FromHex(Settings.FontColor);

            this.BackgroundColor = backgroundColor;
            RunLabel.TextColor = fontColor;
            SaveButton.BackgroundColor = fontColor;
            SaveButton.TextColor = backgroundColor;
        }

        protected override void OnAppearing()
        {
            pickerDataSource = new List<int>();

            base.OnAppearing();
            for (int i = 25; i < 80; i++)
            {
                RunTimes.Items.Add(i.ToString() + "mins");
                pickerDataSource.Add(i);
            }
        }

        public async void OnSave(object sender, EventArgs e)
        {
            int? value = null;

            try
            {
                value = pickerDataSource.ElementAt(RunTimes.SelectedIndex);
            }
            catch (Exception)
            {
                await DisplayAlert("Warning", "Please select a valid time", "OK");
            }


            if (value.HasValue)
            {
                var timeVar = ((30.00 / value.Value) * 1500.00) * 2;
                var hourTime = timeVar - (timeVar * 0.025);
                var metersPerMin = hourTime / 60.00;
                Settings.SwimThresholdPace = metersPerMin;
                Settings.HasCompletedInitialSetup = true;
                _loginManager.Logout();
            }
            else
            {
                await DisplayAlert("Warning", "Please select a valid time", "OK");
            }
        }
    }
}
