using MyFitness.Calculations;
using MyFitness.Data;
using MyFitness.Helpers;
using MyFitness.Model;
using MyFitness.Model.Strava;
using MyFitness.Service;
using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.SfGauge;

using Xamarin.Forms;
using Syncfusion.SfGauge.XForms;

namespace MyFitness.Pages
{
    public partial class MainPage : ContentPage
    {
        private Fitness _fitness;
        private Sql _sqlService;
        private ActivityService _activityService;
        private FitnessModel model;
        public bool isLoading { set { ActIndicator.IsEnabled = value; ActIndicator.IsRunning = value; }}

        /// <summary>
        /// Instantiates a new MainPage.
        /// </summary>
        /// <param name="activityService">The activityService.</param>
        /// <param name="sqlService">The SqlService.</param>
        /// <param name="fitnessService">The fitnessSerivice.</param>
        public MainPage(ActivityService activityService, Sql sqlService)
        {
            _activityService = activityService;
            _sqlService = sqlService;
            _fitness = new Fitness();
            InitializeComponent();
            model = new FitnessModel();
            SetColours();
        }

        private void SetColours()
        {
            this.BackgroundColor = Color.FromHex(Settings.BackgroundColor);

            Color textColor = Color.FromHex(Settings.FontColor);

            AthleteName.TextColor = textColor;
            FitnessLabel.TextColor = textColor;
            FitnessValue.TextColor = textColor;
            FatigueLabel.TextColor = textColor;
            FatigueValue.TextColor = textColor;
            FormLabel.TextColor = textColor;
            FormValue.TextColor = textColor;
            LastSevenDaysLabel.TextColor = textColor;
            LastSevenDays.TextColor = textColor;
            LastFourteenDays.TextColor = textColor;
            LastFourteenDaysLabel.TextColor = textColor;
            LastTwentyEightDays.TextColor = textColor;
            LastTwentyEightDaysLabel.TextColor = textColor;
            RefreshButton.BackgroundColor = Color.FromHex(Settings.FontColor);
            RefreshButton.TextColor = Color.FromHex(Settings.BackgroundColor);
        }

        protected override async void OnAppearing()
        {
            FitnessModel model = new FitnessModel();

            isLoading = true;

            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                model = await _fitness.GetCurrentFitness();
                var athlete = await _activityService.GetAthlete();
                AthleteName.Text = athlete.FirstName;
                GetImprovement();
            }           

            MainLayout.BindingContext = model;

            base.OnAppearing();

            isLoading = false;
        }

        private async void Refresh(object sender, EventArgs e)
        {
            isLoading = true;
            model = await _fitness.GetCurrentFitness();
            var athlete = await _activityService.GetAthlete();
            AthleteName.Text = athlete.FirstName;
            MainLayout.BindingContext = model;
            isLoading = false;
        }

        private void GetImprovement()
        {
            LastSevenDays.Text = _fitness.GetImprovement(7).ToString("+#;-#;0");
            LastFourteenDays.Text = _fitness.GetImprovement(14).ToString("+#;-#;0");
            LastTwentyEightDays.Text = _fitness.GetImprovement(28).ToString("+#;-#;0");
        }
    }
}
