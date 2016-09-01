using MyFitness.Model;
using MyFitness.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyFitness.Model.Strava;
using MyFitness.Data;
using MyFitness.Helpers;

namespace MyFitness.Services
{
    public class ActivityService
    {
        private WebService _webService;
        private Sql _sql;

        public ActivityService()
        {
            _webService = new WebService();
            _sql = new Sql();
        }

        public async Task<List<Activity>> GetAthleteActivities(string authenticationToken)
        {
            var activities = new List<Activity>();

            string url = "https://www.strava.com/api/v3/athlete/activities?include_all_efforts=true&access_token=";

            FitnessResponse response = await _webService.ReceiveRequest(
                url
                + authenticationToken 
                + "&after=" 
                + ConvertToUnixTimestamp(DateTime.Now.AddDays(-42)));

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                return JsonConvert.DeserializeObject<List<Activity>>(response.Content);
            }
            else
            {
                return activities;
            }
        }

        public async Task<MyFitness.Model.Strava.Stream[]> GetActivityStream(int activityId, StreamType type)
        {
            Stream[] s = new Stream[] { new Stream() };

            string url = "https://www.strava.com/api/v3/activities/";

            FitnessResponse response = await _webService.ReceiveRequest(
                url
                + activityId + "/streams/"
                + Enum.GetName(typeof(StreamType), type)
                + "?access_token="
                + Settings.AccessToken);

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                return JsonConvert.DeserializeObject<Stream[]>(response.Content);
            }
            else
            {
                return s;
            }

        }

        public async Task<ActivityZones> GetActivityZones()
        {
            var zones = new ActivityZones();

            string url = "https://www.strava.com/api/v3/athlete/zones?access_token=";

            FitnessResponse response = await _webService.ReceiveRequest(url               
                + Settings.AccessToken);

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                return JsonConvert.DeserializeObject<ActivityZones>(response.Content);
            }
            else
            {
                return zones;
            }

        }

        public async Task<Athlete> GetAthlete()
        {
            var athlete = new Athlete();

            string url = "https://www.strava.com/api/v3/athlete?access_token=";

            FitnessResponse response = await _webService.ReceiveRequest(url + Settings.AccessToken);

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                return JsonConvert.DeserializeObject<Athlete>(response.Content);
            }
            else
            {
                return athlete;
            }
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public enum StreamType
        {
            heartrate,
            time,
            distance
        }
    }
}
