
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyFitness.Model.Strava
{
    public class Activity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("upload_id")]
        public int? UploadId { get; set; }

        [JsonProperty("athlete")]
        public object Athlete { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("distance")]
        public float Distance { get; set; }

        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }

        [JsonProperty("elapsed_time")]
        public int ElapsedTime { get; set; }

        [JsonProperty("total_elevation_gain")]
        public float TotalElevationGain { get; set; }

        [JsonProperty("elev_high")]
        public float ElevationHeight { get; set; }

        [JsonProperty("elev_low")]
        public float ElevationLow { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("start_date_local")]
        public string StartDateLocal { get; set; }

        [JsonProperty("suffer_score")]
        public int? SufferScore { get; set; }

        //suffer_score:	integer

        //timezone:	string
        //start_latlng:	[latitude, longitude]
        //end_latlng:	[latitude, longitude]
        //achievement_count:	integer
        //kudos_count:	integer
        //comment_count:	integer
        //athlete_count:	integer
        //photo_count:	integer
        //total_photo_count:	integer
        //photos:	object
        //map:	object
        //trainer:	boolean
        //commute:	boolean
        //manual:	boolean
        //private:	boolean
        //device_name:	string
        //embed_token:	string
        //flagged:	boolean
        //workout_type:	integer 
        //gear_id:	string
        //gear:	object
        //average_speed:	float
        //max_speed:	float
        //average_cadence:	float
        //average_temp:	float
        //average_watts:	float
        //max_watts:	integer
        //weighted_average_watts:	integer 
        //kilojoules:	float
        //device_watts:	boolean 
        //has_heartrate:	boolean
        //average_heartrate:	float
        //average over moving portion
        //max_heartrate:	integer
        //calories:	float
        //has_kudoed:	boolean 
        //segment_efforts:	array of objects
        //splits_metric:	array of metric split summaries
        //running activities only
        //splits_standard:	array of standard split summaries
        //best_efforts:	array of best effort summaries
    }
}
