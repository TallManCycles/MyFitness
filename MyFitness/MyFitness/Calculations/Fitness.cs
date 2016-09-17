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

[assembly: Xamarin.Forms.Dependency(typeof(ActivityService))]
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

            Athlete athlete = await GetCurrentAthlete();

            if (string.IsNullOrEmpty(Settings.LastCalculationDate))
            {
                model = await InitialCalculation(athlete.Premium);
            }
            else if (await AnyNewActivitesInLastTenDays())
            {
                model = await CalculateFitness(athlete.Premium);
            }
            else if (!string.IsNullOrEmpty(Settings.LastCalculationDate) && DateTime.Parse(Settings.LastCalculationDate).Date < DateTime.Now.Date)
            {
                bool newActivities = await AnyNewActivitesInLastTenDays();

                if (newActivities)
                {
                    model = await CalculateFitness(athlete.Premium);
                }
            }
            else
            {
                model = _sql.GetLatestFitness();
            }            

            return model;
        }

        public int GetImprovement(int daysOfImprovement)
        {
            DateTime date = DateTime.Now.Date.AddDays(-daysOfImprovement);
            IEnumerable<FitnessModel> items = _sql.GetFitnessRange(date, DateTime.Now);

            decimal startImprovement = items.FirstOrDefault().Fitness;
            decimal endImprovement = items.ElementAt(items.Count() - 1).Fitness;
            decimal returnValue = endImprovement - startImprovement;

            return (int)returnValue;
        }

        public int GetConsistency(int daysOfConsistency)
        {
            DateTime date = DateTime.Now.Date.AddDays(-daysOfConsistency);
            IEnumerable<ActivityModel> items = _sql.GetActivityRange(date, DateTime.Now);

            DateTime activityDate = DateTime.MinValue;
            int conistency = 0;

            foreach (ActivityModel a in items)
            {
                if (activityDate.Date != a.Date.Date)
                {
                    conistency += 1;
                }
            }

            var returnValue = ((decimal)conistency / (decimal)daysOfConsistency) * 100M;

            return (int)returnValue;     
        }

        public int GetRiskOfInjury(int daysOfRIsk)
        {
            DateTime date = DateTime.Now.Date.AddDays(-daysOfRIsk);
            IEnumerable<FitnessModel> items = _sql.GetFitnessRange(date, DateTime.Now);

            decimal startRisk = items.FirstOrDefault().Form;
            decimal endRisk = items.ElementAt(items.Count() - 1).Form;
            decimal returnValue = endRisk - startRisk;

            var r = ((decimal)returnValue / (decimal)daysOfRIsk) * 100M;

            return (int)r;
        }

        private async Task<Athlete> GetCurrentAthlete()
        {
            Athlete athlete = new Athlete();

            if (!Settings.HasCompletedInitialSetup)
            {
                athlete = await _activityService.GetAthlete();
                _sql.SaveAthlete(athlete);
            }
            else
            {
                athlete = _sql.GetAthlete();

                if (athlete == null)
                {
                    athlete = await _activityService.GetAthlete();
                    _sql.SaveAthlete(athlete);
                }
            }

            return athlete;
        }

        /// <summary>
        /// Calculates the athletes initial fitness from the last 42 days of activities.
        /// </summary>
        /// <param name="premium">Indicates whether the athlete is premium or not.</param>
        /// <returns>A model containing the final fitness calculations from the last 42 days.</returns>
        public async Task<FitnessModel> InitialCalculation(bool premium)
        {
            ActivityZones zones = new ActivityZones();
            FitnessModel model = new FitnessModel();

            if (!premium)
            {
                zones = await _activityService.GetActivityZones();
            }

            List<Activity> activities = await _activityService.GetAthleteActivities(Settings.AccessToken);

            if (activities == null)
                return model;

            model = await FourtyTwoDayCalculation(activities, premium, zones);

            Settings.LastCalculationDate = DateTime.Now.ToString();
            Settings.CTL = model.Fitness;
            Settings.ATL = model.Fatigue;
            Settings.TSB = model.Form;

            return model;
        }

        private FitnessModel CreateModel(decimal fitness, decimal fatigue, decimal currentTSS, DateTime activityDate)
        {
            FitnessModel model = new FitnessModel();

            model.Date = activityDate;
            model.Fitness = CalculateCTL(fitness, currentTSS);
            model.Form = model.Fitness - fatigue;
            model.Fatigue = CalculateATL(fatigue, currentTSS);

            _sql.SaveFitness(model);

            return model;
        }

        private async Task<IEnumerable<Activity>> GetAllPendingActivities()
        {
            IEnumerable<Activity> ac = await _activityService.GetAthleteActivities(Settings.AccessToken);
            IEnumerable<Activity> actoday = ac.Where(x => DateTime.Parse(x.StartDate).Date > DateTime.Parse(Settings.LastCalculationDate).Date);
            return actoday;
        }

        private async Task<IEnumerable<Activity>> GetAllActivites()
        {
            IEnumerable<Activity> ac = await _activityService.GetAthleteActivities(Settings.AccessToken);
            return ac;
        }

        private async Task<IEnumerable<Activity>> GetLastTenDaysActivities()

        {
            IEnumerable<Activity> ac = await _activityService.GetLastTenDayActivities(Settings.AccessToken);
            return ac;
        }

        private async Task<List<Activity>> GetAthleteActivities()
        {
            return await _activityService.GetAthleteActivities(Settings.AccessToken);
        }

        private decimal CalculateCTL(decimal current, decimal currentSufferScore)
        {
            return (current + ((currentSufferScore - current) * (decimal)(1.00 / 42.00)));
        }

        private decimal CalculateATL(decimal current, decimal currentSufferScore)
        {
            return (current + ((currentSufferScore - current) * (decimal)(1.00 / 7.00)));
        }

        public async Task<FitnessModel> FourtyTwoDayCalculation(List<Activity> activities, bool premium, ActivityZones zones = null)
        {
            FitnessModel model = new FitnessModel();

            decimal CTL = 1;
            decimal ATL = 1;
            decimal TSB = 1;

            for (int i = -42; i <= 0; i++)
            {
                decimal todaysSufferScore = 0;
                decimal activitySufferScore = 0;

                IEnumerable<Activity> dayActivity = activities.Where(x => DateTime.Parse(x.StartDate).Date == DateTime.Now.AddDays(i).Date);

                foreach (Activity ad in dayActivity)
                {
                    if (premium)
                    {
                        if (ad.SufferScore.HasValue)
                        {
                            todaysSufferScore += ad.SufferScore.Value;
                            activitySufferScore = ad.SufferScore.Value;
                        }
                    }
                    else
                    {
                        if (zones != null)
                        {
                            var s = await CalculateTSSFromActivity(ad.Id, zones, ad.Type, ad);
                            todaysSufferScore += s;
                            activitySufferScore = s;
                        }
                    }

                    _sql.SaveActivity(new ActivityModel() { ActivityName = ad.Name,
                        Date = DateTime.Parse(ad.StartDate), TSS = activitySufferScore,
                        StravaActivityId = ad.Id, ActivityType = (int)ad.Type,
                        Distance = ad.Distance / 1000 });
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

        public async Task<FitnessModel> CalculateFitness(bool premium)
        {
            FitnessModel previousDay = new FitnessModel();
            ActivityZones zones = await GetActivityZones();
            decimal DailyTotalTSS = 0.00M;
            decimal activityTSS = 0.00M;
            DateTime date = DateTime.MinValue;
            FitnessModel model = _sql.GetLatestFitness();

            IEnumerable<Activity> dayActivity = await GetAllPendingActivities();

            foreach (Activity ad in dayActivity)
            {
                activityTSS = 0.00M;

                if (date == DateTime.MinValue)
                {
                    date = DateTime.Parse(ad.StartDate);
                }
                else if (date.Date != DateTime.Parse(ad.StartDate).Date)
                {
                    model = CreateModel(model.Fitness, model.Fatigue, DailyTotalTSS, DateTime.Parse(ad.StartDate));

                    date = DateTime.Parse(ad.StartDate);

                    DailyTotalTSS = 0.00M;
                }

                if (premium)
                {
                    if (ad.SufferScore.HasValue)
                    {
                        DailyTotalTSS += ad.SufferScore.Value;
                        activityTSS = ad.SufferScore.Value;
                    }
                }
                else
                {
                    var s = await CalculateTSSFromActivity(ad.Id, zones, ad.Type, ad);
                    DailyTotalTSS += s;
                    activityTSS = s;
                }

                _sql.SaveActivity(new ActivityModel() { ActivityName = ad.Name,
                    Date = DateTime.Parse(ad.StartDate),
                    TSS = activityTSS, StravaActivityId = ad.Id,
                    ActivityType = (int)ad.Type,
                    Distance = ad.Distance / 1000});
            }            

            model = CreateModel(model.Fitness, model.Fatigue, DailyTotalTSS, DateTime.Now);

            Settings.LastCalculationDate = DateTime.Now.ToString();

            return model;
        }        

        private async Task<ActivityZones> GetActivityZones()
        {
            return await _activityService.GetActivityZones();
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

        private async Task<bool> AnyNewActivitesInLastTenDays()
        {
            if (string.IsNullOrEmpty(Settings.AccessToken))
                return true;

            IEnumerable<Activity> activites = await GetLastTenDaysActivities();

            IEnumerable<ActivityModel> savedActivites = _sql.GetActivities().Where(x => x.Date.Date >= DateTime.Now.AddDays(-10).Date);

            var result = savedActivites.Where(p => !activites.Any(p2 => p2.Id == p.StravaActivityId));

            if (result.Count() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
