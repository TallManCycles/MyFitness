using MyFitness.Data;
using MyFitness.Helpers;
using MyFitness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class Activities : ContentPage
    {
        private Sql _sql;
        private List<ActivitiesList> activityList;

        public Activities()
        {
            _sql = new Sql();
            InitializeComponent();

            this.BackgroundColor = Color.FromHex(Settings.BackgroundColor);

            activityList = new List<ActivitiesList>();

            List<ActivityModel> model = _sql.GetActivities().ToList();

            foreach (ActivityModel activity in model)
            {
                var a = new ActivitiesList();
                a.ActivityName = activity.ActivityName;
                a.SufferScore = activity.TSS;
                a.ActivityDate = activity.Date;
                a.Distance = activity.Distance;
                activityList.Add(a);
            }

            if (activityList.Count() > 0)
            {
                ActivityView.ItemsSource = activityList.OrderByDescending(x => x.ActivityDate);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void SelectedCell(object sender, EventArgs e)
        {
            if (ActivityView.SelectedItem != null)
            {
                ActivityView.SelectedItem = null;
            }
        }
    }

    public class ActivitiesList
    {
        public string ActivityName { get; set; }

        public decimal SufferScore { get; set; }

        public DateTime ActivityDate { get; set; }

        public float Distance { get; set; }
    }
}
