using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Model.Strava
{
    public class ActivityZones
    {
        [JsonProperty("heart_rate")]
        public HeartRateZones HeartRate { get; set; }
    }

    public class HeartRateZones
    {
        [JsonProperty("custom_zones")]
        public bool CustomZones { get; set; }

        [JsonProperty("zones")]
        public List<Zones> zones { get; set; }
    }

    public class Zones
    {
        [JsonProperty("min")]
        public int Min { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }
    }
}
