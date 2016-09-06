using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Model
{
    public class ActivityModel
    {
        [PrimaryKey, AutoIncrement]
        public int ActivityId { get; set; }

        public string ActivityName { get; set; }

        public decimal TSS { get; set; }

        public DateTime Date { get; set; }

        public int StravaActivityId { get; set; }
    }
}
