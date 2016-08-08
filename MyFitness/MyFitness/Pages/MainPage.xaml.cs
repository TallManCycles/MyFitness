using MyFitness.Data;
using MyFitness.Model;
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
        private Sql _sql;
        private WebService _webService;

        public MainPage()
        {
            InitializeComponent();
            _webService = new WebService();
            _sql = new Sql();
        }

        protected override void OnAppearing()
        {
            FitnessModel model = new FitnessModel()
            {
                Fitness = 44,
                Fatigue = 68,
                Form = 44 - 68,
                Date = DateTime.Now,
                Id = 1
            };

            MainLayout.BindingContext = model;

            //_webService.Get("https://www.strava.com/api/v3/activities/665030482?access_token=" + Helpers.Settings.AccessToken);

            base.OnAppearing();
        }

        private async void ManualEntry(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TSSEntry.Text))
            {
                var items = _sql.GetFitnessItems();

                if (items.Any())
                {
                    foreach (FitnessModel i in items)
                    {
                        if (i.Date.Date == DateTime.Now.Date)
                        {
                            var result = await DisplayAlert("Confirm Entry", "You already have an entry for today. Would you like to overwrite?", "Yes", "No");

                            if (!result)
                                return;
                        }
                    }

                    var models = items.OrderByDescending(x => x.Date);



                }
            }
        }
    }
}
