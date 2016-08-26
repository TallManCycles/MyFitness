using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MyFitness.Model
{
    public class FitnessModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Fitness { get; set; }

        public decimal Fatigue { get; set; }

        public decimal Form { get; set; }
    }
}
