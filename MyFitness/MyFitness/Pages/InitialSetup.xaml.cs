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
    public partial class InitialSetup : ContentPage
    {
        private List<int> pickerDataSource;
        private ILoginManager _loginManager;

        public InitialSetup(ILoginManager loginManager)
        {
            InitializeComponent();
            _loginManager = loginManager;
        }

        protected override void OnAppearing()
        {
            pickerDataSource = new List<int>();

            base.OnAppearing();
            for (int i = 10; i < 55; i++)
            {
                SwimTimes.Items.Add(i.ToString() + "mins");
                pickerDataSource.Add(i);
            }
        }

        public void SwimToggle(object sender, EventArgs e)
        {
            SwimTimes.IsEnabled = true;
            SwimLabel.IsEnabled = true;                      
        }

        public async void OnSave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FirstName.Text))
            {
                await DisplayAlert("Warning", "Please enter a first name", "OK");
                return;
            }

            if (SwimSwitch.IsToggled)
            {
                int? value = null;

                try
                {
                    value = pickerDataSource.ElementAt(SwimTimes.SelectedIndex);
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
                    App.Instance.Logout();
                }
                else
                {
                    await DisplayAlert("Warning", "Please select a valid time", "OK");
                }
            }
            else
            {
                Settings.HasCompletedInitialSetup = true;
                _loginManager.Logout();
            }
        }
    }
}
