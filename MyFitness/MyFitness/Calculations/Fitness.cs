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
        const float CTLConst = 1 / 42;
        const float ATLConst = 1 / 7;

        public static FitnessModel InitialCalculation(List<Activity> activities)
        {
            FitnessModel model = new FitnessModel();

            float CTL = 1;
            float ATL = 1;
            float TSB = 1;

            if (activities == null)
                return model;
            if (Settings.HasInitialCalculation)
                return model;

                DateTime startDate = DateTime.Now.AddDays(-42);

            for (int i = -42; i < 0; i++)
            {
                double todaysSufferScore = 0;

                IEnumerable<Activity> dayActivity = activities.Where(x => DateTime.Parse(x.StartDate).Date == DateTime.Now.AddDays(i).Date);

                foreach (Activity ad in dayActivity)
                {
                    if (ad.SufferScore.HasValue)
                        todaysSufferScore += ad.SufferScore.Value;
                }

                CTL = CalculateCTL(CTL, todaysSufferScore);
                ATL = CalculateATL(ATL, todaysSufferScore);

                TSB = CTL - ATL;
            }

            model.Fitness = CTL;
            model.Fatigue = ATL;
            model.Form = TSB;
            model.Id = 1;
            model.Date = DateTime.Now.Date;

            return model;
        }

        private static float CalculateCTL(double current, double currentSufferScore)
        {
            return (float)(current + ((currentSufferScore - current) * (1.00 / 42.00)));
        }

        private static float CalculateATL(double current, double currentSufferScore)
        {
            return (float)(current + ((currentSufferScore - current) * (1.00 / 7.00)));
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
