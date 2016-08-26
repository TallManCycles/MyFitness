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

namespace MyFitness.Calculations
{
    public class Fitness
    {
        private Sql _sql;
        private ActivityService _activityService;

        public Fitness()
        {
            _sql = new Sql();
            _activityService = new ActivityService();
        }

        /// <summary>
        /// Gets the athletes current fitness
        /// </summary>
        /// <returns>A model containing the athletes current fitness model.</returns>
        public async Task<FitnessModel> GetCurrentFitness()
        {
            var model = new FitnessModel() { Fitness = Settings.CTL, Fatigue = Settings.ATL, Form = Settings.TSB, Id = 1, Date = DateTime.Now };

            var athlete = await _activityService.GetAthlete();

            if (athlete.Premium)
            {
                if (string.IsNullOrEmpty(Settings.LastCalculationDate))
                {
                    model = await InitialCalculationPremium();
                }
                else if (!string.IsNullOrEmpty(Settings.LastCalculationDate) && DateTime.Parse(Settings.LastCalculationDate).Date != DateTime.Now.Date)
                {
                    model = await CalculateFitnessPremium();
                }
                else
                {
                    model = _sql.GetLatestFitness();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Settings.LastCalculationDate))
                {
                    model = await InitialCalculation();
                }

                if (!string.IsNullOrEmpty(Settings.LastCalculationDate) && DateTime.Parse(Settings.LastCalculationDate).Date != DateTime.Now.Date)
                {
                    model = await CalculateFitness();
                }
            }

            return model;
        }

        /// <summary>
        /// Calculates the athletes initial fitness from the last 42 days of activities.
        /// </summary>
        /// <returns>A model containing the final fitness calculations from the last 42 days.</returns>
        private async Task<FitnessModel> InitialCalculationPremium()
        {
            FitnessModel model = new FitnessModel();

            List<Activity> activities = await _activityService.GetAthleteActivities(Settings.AccessToken);

            if (activities == null)
                return model;

            model = await FourtyTwoDayCalculation(activities, true);

            Settings.LastCalculationDate = DateTime.Now.ToString();
            Settings.CTL = model.Fitness;
            Settings.ATL = model.Fatigue;
            Settings.TSB = model.Form;

            return model;
        }

        private async Task<FitnessModel> CalculateFitnessPremium()
        {
            FitnessModel previousDay = new FitnessModel();

            decimal CurrentTSS = (decimal)0.00;

            List<Activity> activities = await _activityService.GetAthleteActivities(Settings.AccessToken);

            IEnumerable<Activity> dayActivity = activities.Where(x => DateTime.Parse(x.StartDate).Date == DateTime.Now.Date);

            foreach (Activity ad in dayActivity)
            {
                if (ad.SufferScore.HasValue)
                    CurrentTSS += ad.SufferScore.Value;

                _sql.SaveActivity(new ActivityModel() { ActivityName = ad.Name, Date = DateTime.Parse(ad.StartDate), TSS = CurrentTSS });
            }

            previousDay.Fitness = Settings.CTL;
            previousDay.Fatigue = Settings.ATL;
            previousDay.Form = Settings.TSB;
            previousDay.Date = DateTime.Now.AddDays(-1);

            FitnessModel model = new FitnessModel();

            if (previousDay != null)
            {
                model.Fitness = previousDay.Fitness + ((CurrentTSS - previousDay.Fitness) * (decimal)(1.00 / 42.00));
                model.Form = model.Fitness - previousDay.Fatigue;
                model.Fatigue = previousDay.Fatigue + ((CurrentTSS - previousDay.Fatigue) * (decimal)(1.00 / 7.00));
                
                model.Date = DateTime.Now.Date;
                model.Id = previousDay.Id + 1;

                return model;
            }
            else
            {
                return model;
            }            
        }

        private decimal CalculateCTL(decimal current, decimal currentSufferScore)
        {
            return (current + ((currentSufferScore - current) * (decimal)(1.00 / 42.00)));
        }

        private decimal CalculateATL(decimal current, decimal currentSufferScore)
        {
            return (current + ((currentSufferScore - current) * (decimal)(1.00 / 7.00)));
        }

        private async Task<FitnessModel> FourtyTwoDayCalculation(List<Activity> activities, bool premium, ActivityZones zones = null)
        {
            FitnessModel model = new FitnessModel();

            decimal CTL = 1;
            decimal ATL = 1;
            decimal TSB = 1;

            for (int i = -42; i <= 0; i++)
            {
                decimal todaysSufferScore = 0;

                IEnumerable<Activity> dayActivity = activities.Where(x => DateTime.Parse(x.StartDate).Date == DateTime.Now.AddDays(i).Date);

                foreach (Activity ad in dayActivity)
                {
                    if (premium)
                    {
                        if (ad.SufferScore.HasValue)
                            todaysSufferScore += ad.SufferScore.Value;
                    }
                    else
                    {
                        if (zones != null)
                        {
                            todaysSufferScore += await CalculateTSSFromActivity(ad.Id, zones, ad.Type, ad);
                        }
                    }

                    _sql.SaveActivity(new ActivityModel() { ActivityName = ad.Name, Date = DateTime.Parse(ad.StartDate), TSS = todaysSufferScore });
                }

                CTL = CalculateCTL(CTL, todaysSufferScore);
                TSB = CTL - ATL;
                ATL = CalculateATL(ATL, todaysSufferScore);

                //Save the fitness to the database
                _sql.SaveFitness(new FitnessModel() { Fitness = CTL, Fatigue = ATL, Form = TSB, Date = DateTime.Now.AddDays(i) });             
            }

            model.Fitness = CTL;
            model.Fatigue = ATL;
            model.Form = TSB;
            model.Id = 1;
            model.Date = DateTime.Now;

            return model;
        }

        private async Task<FitnessModel> InitialCalculation()
        {
            FitnessModel model = new FitnessModel();

            ActivityZones zones = await _activityService.GetActivityZones();            

            List<Activity> activities = await _activityService.GetAthleteActivities(Settings.AccessToken);

            if (activities == null)
                return model;

            model = await FourtyTwoDayCalculation(activities, false, zones);

            Settings.LastCalculationDate = DateTime.Now.ToString();
            Settings.CTL = model.Fitness;
            Settings.ATL = model.Fatigue;
            Settings.TSB = model.Form;

            return model;
        }

        private async Task<FitnessModel> CalculateFitness()
        {
            FitnessModel previousDay = new FitnessModel();

            decimal CurrentTSS = (decimal)0.00;

            List<Activity> activities = await _activityService.GetAthleteActivities(Settings.AccessToken);

            ActivityZones zones = await _activityService.GetActivityZones();

            IEnumerable<Activity> dayActivity = activities.Where(x => DateTime.Parse(x.StartDate).Date > DateTime.Parse(Settings.LastCalculationDate).Date);

            foreach (Activity ad in dayActivity)
            {
                CurrentTSS += await CalculateTSSFromActivity(ad.Id, zones, ad.Type, ad);

                _sql.SaveActivity(new ActivityModel() { ActivityName = ad.Name, Date = DateTime.Parse(ad.StartDate), TSS = CurrentTSS });
            }

            previousDay.Fitness = Settings.CTL;
            previousDay.Fatigue = Settings.ATL;
            previousDay.Form = Settings.TSB;
            previousDay.Date = DateTime.Now.AddDays(-1);

            FitnessModel model = new FitnessModel();

            if (previousDay != null)
            {
                model.Fitness = previousDay.Fitness + ((CurrentTSS - previousDay.Fitness) * (decimal)(1.00 / 42.00));
                model.Form = model.Fitness - previousDay.Fatigue;
                model.Fatigue = previousDay.Fatigue + ((CurrentTSS - previousDay.Fatigue) * (decimal)(1.00 / 7.00));

                model.Date = DateTime.Now.Date;
                model.Id = previousDay.Id + 1;

                Settings.LastCalculationDate = DateTime.Now.ToString();

                _sql.SaveFitness(model);

                return model;
            }
            else
            {
                return model;
            }
        }

        private async Task<decimal> CalculateTSSFromActivity(int id, ActivityZones zones, ActType activityType, Activity activity)
        {
            decimal tss = 0.00M;
            decimal[] timeInZones = new decimal[] { 0.0M, 0.0M, 0.0M, 0.0M, 0.0M };

            //Gets the activity streams for the activity id
            Stream[] hrALL = await _activityService.GetActivityStream(id, ActivityService.StreamType.heartrate);
            Stream[] timeALL = await _activityService.GetActivityStream(id, ActivityService.StreamType.time);            

            //Gets the steeam assiciated with the correct types
            Stream heartRate = hrALL.FirstOrDefault(x => x.Type == "heartrate");
            Stream time = timeALL.FirstOrDefault(x => x.Type == "time");       

            if (heartRate == null || time == null)
                return tss;

            //Create a keyvalue pair list of heart rate and time in that order.
            List<KeyValuePair<decimal, decimal>> heartRateOverTime = new List<KeyValuePair<decimal, decimal>>();

            decimal h = 0;
            decimal t = 0;

            for (int i = 0; i < time.DataValues.Count() - 1; i++)
            {
                if (i == 0 )
                {
                    h = heartRate.DataValues[i];
                    t = time.DataValues[i];
                }
                else
                {
                    h = heartRate.DataValues[i];
                    t = time.DataValues[i] - time.DataValues[i - 1];
                }          
                      
                heartRateOverTime.Add(new KeyValuePair<decimal, decimal>(h, t));
            }

            var a = zones.HeartRate.zones[0].Max;
            var b = zones.HeartRate.zones[1].Max;
            var c = zones.HeartRate.zones[2].Max;
            var d = zones.HeartRate.zones[3].Max;
            var e = zones.HeartRate.zones[4].Min;


            foreach (KeyValuePair<decimal, decimal> values in heartRateOverTime)
            {
                if (values.Key < a)
                {
                    //in first zone
                    timeInZones[0] += values.Value;
                }
                else if (values.Key >= a && values.Key < b)
                {
                    //in second zone
                    timeInZones[1] += values.Value;
                }
                else if (values.Key >= b && values.Key < c)
                {
                    //in third zone
                    timeInZones[2] += values.Value;
                }
                else if (values.Key >= c && values.Key <= d)
                {
                    //in fourth zone
                    timeInZones[3] += values.Value;
                }
                else if (values.Key > e)
                {
                    timeInZones[4] += values.Value;
                }
            }

            if (activityType == ActType.Ride)
            {
                tss = (((timeInZones[0] / 60.00M) / 60.00M) * 15.0M) +
                    (((timeInZones[1] / 60.00M) / 60.00M) * 30.0M) +
                    (((timeInZones[2] / 60.00M) / 60.00M) * 60.0M) +
                    (((timeInZones[3] / 60.00M) / 60.00M) * 120.0M) +
                    (((timeInZones[4] / 60.00M) / 60.00M) * 240.0M);
            }
            else if (activityType == ActType.Run)
            {
                tss = (((timeInZones[0] / 60.00M) / 60.00M) * 30.0M) +
                    (((timeInZones[1] / 60.00M) / 60.00M) * 60.0M) +
                    (((timeInZones[2] / 60.00M) / 60.00M) * 120.0M) +
                    (((timeInZones[3] / 60.00M) / 60.00M) * 240.0M) +
                    (((timeInZones[4] / 60.00M) / 60.00M) * 480.0M);
            }
            else if (activityType == ActType.Swim)
            {
                var metersPerMin = activity.AverageSpeed * 60.00;
                var IF = metersPerMin / Settings.SwimThresholdPace;
                tss = (decimal)(IF * IF * IF) * (decimal)((activity.MovingTime / 60.00) / 60.00) * 100.00M;
            }
            else
            {
                //No tss calculations for other activities yet;
            }

            return tss;

        }
    }
}
