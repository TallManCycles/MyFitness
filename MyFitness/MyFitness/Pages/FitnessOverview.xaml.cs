using MyFitness.Calculations;
using MyFitness.Data;
using MyFitness.Helpers;
using MyFitness.Model;
using Syncfusion.RangeNavigator.XForms;
using Syncfusion.SfChart.XForms;
using Syncfusion.SfGauge.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class FitnessOverview : ContentPage
    {
        public bool isLoading { set { ActIndicator.IsRunning = value; ActIndicator.IsVisible = value; } }

        FitnessModel model = new FitnessModel();
        private Fitness _fitness;

        public FitnessOverview()
        {
            
            _fitness = new Fitness();
            InitializeComponent();
            this.BackgroundColor = Color.FromHex(Settings.BackgroundColor);
        }

        private void SetColours()
        {
            this.BackgroundColor = Color.FromHex(Settings.BackgroundColor);
        }

        protected async override void OnAppearing()
        {
            isLoading = true;
            model = await _fitness.GetCurrentFitness();

            var now = DateTime.Now;

            var lseven = new Label() { Text = "Past 7 Days" };
            lseven.HorizontalOptions = LayoutOptions.CenterAndExpand;
            var sevenDayChart = CreateLineChart(DateTime.Now.AddDays(-7), now);
            var lfourteen = new Label() { Text = "Past 28 Days" };
            lfourteen.HorizontalOptions = LayoutOptions.CenterAndExpand;
            var fourteenDayChart = CreateLineChart(DateTime.Now.AddDays(-14), now);
            var lmonth = new Label() { Text = "Last 2 Months" };
            lmonth.HorizontalOptions = LayoutOptions.CenterAndExpand;
            var fourtyTwoDayChart = CreateLineChart(DateTime.Now.AddMonths(-2), now);

            MainLayout.Children.Add(lseven);
            MainLayout.Children.Add(sevenDayChart);
            MainLayout.Children.Add(lfourteen);
            MainLayout.Children.Add(fourteenDayChart);
            MainLayout.Children.Add(lmonth);
            MainLayout.Children.Add(fourtyTwoDayChart);


            SetColours();

            base.OnAppearing();

            isLoading = false;
        }

        private SfDateTimeRangeNavigator CreateLineChart(DateTime fromDate, DateTime toDate)
        {
            SfDateTimeRangeNavigator rangeNavigator = new SfDateTimeRangeNavigator();

            rangeNavigator.HeightRequest = 200;

            rangeNavigator.Minimum = fromDate;
            rangeNavigator.Maximum = toDate;
            rangeNavigator.ViewRangeStart = fromDate;
            rangeNavigator.ViewRangeEnd = toDate;
            rangeNavigator.BackgroundColor = Color.FromHex(Settings.BackgroundColor);

            rangeNavigator.EnableTooltip = true;

            var data = new DataModel(fromDate, toDate);
            rangeNavigator.ItemsSource = data.Data;
            rangeNavigator.XBindingPath = "Date";
            rangeNavigator.YBindingPath = "Value";

            return rangeNavigator;
    }

        private enum FitnessType
        {
            fitness,
            fatigue,
            form
        }
    }

    public class DataModel
    {
        private Sql _sql;

        public ObservableCollection<Model> Data { get; set; }

        public DataModel(DateTime fromdate, DateTime toDate)
        {
            _sql = new Sql();

            var items = _sql.GetFitnessRange(fromdate, toDate);

            Data = new ObservableCollection<Model>();

            foreach (FitnessModel a in items)
            {
                Data.Add(new Model(a.Date, (double)a.Fitness));
            }
        }
    }

    public class Model
    {
        public DateTime Date { get; set; }

        public double Value { get; set; }

        public Model(DateTime dateTime, double value)
        {
            Date = dateTime;
            Value = value;
        }
    }
}
