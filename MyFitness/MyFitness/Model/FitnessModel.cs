using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Model
{
    public class FitnessModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double Fitness { get; set; }

        public double Fatigue { get; set; }

        public double Form { get; set; }
    }
}
