using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Model.Strava
{
    public class Stream
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public decimal[] DataValues { get; set; }

        //[JsonProperty("series_type")]
        //public string SeriesType { get; set; }

        //[JsonProperty("original_size")]
        //public int OriginalSize { get; set; }

        //[JsonProperty("resolution")]
        //public string Resolution { get; set; }

    }

    [JsonArray]
    public class Data
    {        
        public decimal value { get; set; }
    }
}
