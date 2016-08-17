using MyFitness.Data;
using MyFitness.Helpers;
using MyFitness.Model;
using MyFitness.Model.Strava;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Calculations
{
    public class Fitness
    {
        const decimal CTLConst = 1 / 42;
        const decimal ATLConst = 1 / 7;
        private Sql _sql;

        public Fitness()
        {
            _sql = new Sql();
        }
        public FitnessModel InitialCalculation(List<Activity> activities)
        {
            FitnessModel model = new FitnessModel();

            decimal CTL = 1;
            decimal ATL = 1;
            decimal TSB = 1;

            if (activities == null)
                return model;
            if (Settings.HasInitialCalculation)
                return model;

                DateTime startDate = DateTime.Now.AddDays(-42);

            for (int i = -42; i <= 0; i++)
            {
                decimal todaysSufferScore = 0;

                IEnumerable<Activity> dayActivity = activities.Where(x => DateTime.Parse(x.StartDate).Date == DateTime.Now.AddDays(i).Date);

                foreach (Activity ad in dayActivity)
                {
                    if (ad.SufferScore.HasValue)
                        todaysSufferScore += ad.SufferScore.Value;
                }

                CTL = CalculateCTL(CTL, todaysSufferScore);
                TSB = CTL - ATL;
                ATL = CalculateATL(ATL, todaysSufferScore);

                FitnessModel f = new FitnessModel() { Id = i, Date = DateTime.Now.AddDays(i), Fitness = CTL, Fatigue = ATL, Form = CTL };
                SaveFitness(f);                
            }

            model.Fitness = CTL;
            model.Fatigue = ATL;
            model.Form = TSB;
            model.Id = 1;
            model.Date = DateTime.Now.Date;

            return model;
        }

        private decimal CalculateCTL(decimal current, decimal currentSufferScore)
        {
            return (current + ((currentSufferScore - current) * (decimal)(1.00 / 42.00)));
        }

        private decimal CalculateATL(decimal current, decimal currentSufferScore)
        {
            return (current + ((currentSufferScore - current) * (decimal)(1.00 / 7.00)));
        }

        private bool SaveFitness(FitnessModel f)
        {
            _sql.InsertFitness(f);
            return true;
        }

        public FitnessModel CalculateFitness(int CurrentTSS, FitnessModel previousDay)
        {
            FitnessModel model = new FitnessModel();

            if (previousDay != null)
            {
                model.Fitness = previousDay.Fitness + ((CurrentTSS - previousDay.Fitness) * CTLConst);
                model.Fatigue = previousDay.Fatigue + ((CurrentTSS - previousDay.Fatigue) * ATLConst);
                model.Form = model.Fitness - model.Fatigue;
                model.Date = DateTime.Now.Date;
                model.Id = previousDay.Id + 1;

                return model;
            }
            else
            {
                return model;
            }            
        }
    }
}
