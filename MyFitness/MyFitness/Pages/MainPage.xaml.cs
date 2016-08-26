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
        private Sql _sql;
        private ActivityService _webService;
        private FitnessModel model;


        public MainPage()
        {            
            _webService = new ActivityService();
            _sql = new Sql();
            _fitness = new Fitness();
            InitializeComponent();
            model = new FitnessModel();
        }

        protected override async void OnAppearing()
        {
            model = await _fitness.GetCurrentFitness();
            var athlete = await _webService.GetAthlete();
            AthleteName.Text = athlete.FirstName;

            MainLayout.BindingContext = model;

            base.OnAppearing();
        }

        private async void Refresh(object sender, EventArgs e)
        {
            model = await _fitness.GetCurrentFitness();
            var athlete = await _webService.GetAthlete();
            AthleteName.Text = athlete.FirstName;
        }
    }
}
