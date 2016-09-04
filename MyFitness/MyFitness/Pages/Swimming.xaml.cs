using MyFitness.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class Swimming : ContentPage
    {
        private List<int> pickerDataSource;

        public Swimming()
        {
            InitializeComponent();

            Color backgroundColor = Color.FromHex(Settings.BackgroundColor);
            Color fontColor = Color.FromHex(Settings.FontColor);

            SaveButton.BackgroundColor = Color.FromHex(Settings.FontColor);
            SaveButton.TextColor = Color.FromHex(Settings.BackgroundColor);

            this.BackgroundColor = backgroundColor;
            SwimLabel.TextColor = fontColor;
            SwimPrompt.TextColor = fontColor;
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

                    MessagingCenter.Send<ContentPage>(this, "Running");
                }
                else
                {
                    await DisplayAlert("Warning", "Please select a valid time", "OK");
                }
            }
            else
            {
                MessagingCenter.Send<ContentPage>(this, "Running");
            }
        }
    }
}
