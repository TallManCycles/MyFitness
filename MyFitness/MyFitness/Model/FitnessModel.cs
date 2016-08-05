using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Model
{
    class FitnessModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Fitness { get; set; }

        public int Fatigue { get; set; }

        public int Form { get; set; }
    }
}
