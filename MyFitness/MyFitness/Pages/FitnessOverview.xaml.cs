﻿using MyFitness.Calculations;
using MyFitness.Data;
using MyFitness.Model;
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
        SfCircularGauge fitnessguague;
        SfCircularGauge fatigueguage;
        SfCircularGauge formguague;

        FitnessModel model = new FitnessModel();
        private Fitness _fitness;

        public FitnessOverview()
        {
            _fitness = new Fitness();
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            model = await _fitness.GetCurrentFitness();

            fitnessguague = CreateGuague("Fitness", (double)model.Fitness, 0, 50, 10, FitnessType.fitness);
            fatigueguage = CreateGuague("Fatigue", (double)model.Fatigue, 0, 50, 10, FitnessType.fatigue);
            formguague = CreateGuague("Form", (double)model.Form, -25, 25, 10, FitnessType.form);

            var chart = CreateChart("Hello");

            MainLayout.Children.Add(fitnessguague);
            MainLayout.Children.Add(fatigueguage);
            MainLayout.Children.Add(formguague);
            MainLayout.Children.Add(chart);
            base.OnAppearing();
        }

        private SfCircularGauge CreateGuague(string headerText, double value, int minRange, int maxRange, int interval, FitnessType type)
        {
            SfCircularGauge guage = new SfCircularGauge();

            Header header = new Header();
            header.Text = headerText + " " + value.ToString("0.00");
            header.TextSize = 20;
            header.Position = Device.OnPlatform(iOS: new Point(.3, .7), Android: new Point(0.5, 0.7), WinPhone: new Point(0.38, 0.7));
            header.ForegroundColor = Color.Gray;
            guage.Headers.Add(header);

            Scale scale = new Scale();
            scale.StartValue = minRange;
            scale.EndValue = maxRange;
            scale.Interval = interval;
            scale.StartAngle = 135;
            scale.SweepAngle = 270;
            scale.RimThickness = 20;
            scale.LabelColor = Color.White;
            scale.LabelOffset = 0.1;
            scale.MinorTicksPerInterval = 1;

            switch (type)
            {
                case FitnessType.fitness:
                    Range badRange = new Range();
                    badRange.StartValue = minRange;
                    badRange.EndValue = maxRange - (0.5 * maxRange);
                    badRange.Color = Color.Blue;
                    badRange.Thickness = 10;
                    scale.Ranges.Add(badRange);
                    Range goodRange = new Range();
                    goodRange.StartValue = maxRange - (0.5 * maxRange);
                    goodRange.EndValue = maxRange;
                    goodRange.Color = Color.Green;
                    goodRange.Thickness = 10;
                    scale.Ranges.Add(goodRange);
                    break;
                case FitnessType.fatigue:
                    Range badRange1 = new Range();
                    badRange1.StartValue = maxRange - (0.5 * maxRange);
                    badRange1.EndValue = maxRange;
                    badRange1.Color = Color.Blue;
                    badRange1.Thickness = 10;
                    scale.Ranges.Add(badRange1);
                    Range goodRange1 = new Range();
                    goodRange1.StartValue = minRange;
                    goodRange1.EndValue = maxRange - (0.5 * maxRange);
                    goodRange1.Color = Color.Green;
                    goodRange1.Thickness = 10;
                    scale.Ranges.Add(goodRange1);
                    break;
                case FitnessType.form:
                    Range badRange2 = new Range();
                    badRange2.StartValue = minRange;
                    badRange2.EndValue = minRange + (0.4 * maxRange);
                    badRange2.Color = Color.Blue;
                    badRange2.Thickness = 10;
                    scale.Ranges.Add(badRange2);
                    Range goodRange2 = new Range();
                    goodRange2.StartValue = minRange + (0.4 * maxRange);
                    goodRange2.EndValue = maxRange;
                    goodRange2.Color = Color.Green;
                    goodRange2.Thickness = 10;
                    scale.Ranges.Add(goodRange2);
                    break;
                default:
                    break;
            }

            List<Pointer> pointers = new List<Pointer>();
            NeedlePointer needlePointer = new NeedlePointer();
            needlePointer.Value = value;
            needlePointer.Color = Color.Gray;
            needlePointer.KnobRadius = 10;
            needlePointer.KnobColor = Color.FromHex("#2bbfb8");
            needlePointer.Thickness = 2;
            needlePointer.LengthFactor = 0.8;
            needlePointer.Type = PointerType.Triangle;
            pointers.Add(needlePointer);
            scale.Pointers = pointers;
            guage.Scales.Add(scale);

            return guage;
        }

        private SfChart CreateChart(string title)
        {
            SfChart chart = new SfChart();

            //Initializing Primary Axis
            CategoryAxis primaryAxis = new CategoryAxis();
            primaryAxis.Title = new ChartAxisTitle()
            {
                Text = "Fittness"
            };
            chart.PrimaryAxis = primaryAxis;

            //Initializing Secondary Axis
            NumericalAxis secondaryAxis = new NumericalAxis();
            secondaryAxis.Title = new ChartAxisTitle()
            {
                Text = "Date"
            };
            chart.SecondaryAxis = secondaryAxis;

            chart.Title = new ChartTitle()
            {
                Text = title
            };

            var d = new DataModel();

            chart.Series.Add(new ColumnSeries()
            {
                ItemsSource = d.Data
            });

            return chart;
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

        public ObservableCollection<ChartDataPoint> Data { get; set; }

        public DataModel()
        {
            _sql = new Sql();

            Data = new ObservableCollection<ChartDataPoint>();
            foreach (ActivityModel a in _sql.GetActivities())
            {
                Data.Add(new ChartDataPoint(a.Date.ToString("dd"), (double)a.TSS));
            }
        }
    }
}