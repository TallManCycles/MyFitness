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

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class MainPage : ContentPage
    {
        private object Webservice;
        private Fitness _fitness;
        private Sql _sql;
        private ActivityService _webService;

        public MainPage()
        {
            InitializeComponent();
            _webService = new ActivityService();
            _sql = new Sql();
            _fitness = new Fitness();

        }

        protected override async void OnAppearing()
        {
            List<Activity> activities = await _webService.GetAthleteActivities(Settings.AccessToken);

            FitnessModel model = _fitness.InitialCalculation(activities);

            MainLayout.BindingContext = model;

            base.OnAppearing();
        }

        private async void Refresh(object sender, EventArgs e)
        {
            List<Activity> activities = await _webService.GetAthleteActivities(Settings.AccessToken);

            FitnessModel model = _fitness.InitialCalculation(activities);

            MainLayout.BindingContext = model;
        }
    }
}
