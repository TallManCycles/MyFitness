using MyFitness.Helpers;
using MyFitness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Calculations
{
    public class Fitness
    {
        const double CTLConst = 1 / 42;
        const double ATLConst = 1 / 7;

        public Fitness()
        {

        }

        public FitnessModel CalculateFitness(int CurrentTSS, FitnessModel previousDay)
        {
            FitnessModel model = new FitnessModel();

            if (Settings.HasCompletedSevenDays && previousDay != null)
            {
                model.Fitness = previousDay.Fitness + ((CurrentTSS - previousDay.Fitness) * CTLConst);
                model.Fatigue = previousDay.Fatigue + ((CurrentTSS - previousDay.Fatigue) * ATLConst);
                model.Form = model.Fitness - model.Fatigue;

                return model;
            }
            else
            {
                return model;
                //Do the seven day average to calulate Fitness
            }            
        }
    }
}
