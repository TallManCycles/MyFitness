using MyFitness.Data;
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

        public Activities()
        {
            _sql = new Sql();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            List<ActivityModel> model = _sql.GetActivities().ToList();
            ActivityView.ItemsSource = model.OrderByDescending(x => x.ActivityId);
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
}
