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

            FitnessResponse response = await _webService.ReceiveRequest(
                "https://www.strava.com/api/v3/athlete/activities?include_all_efforts=true&access_token=" 
                + authenticationToken 
                + "&after=" 
                + ConvertToUnixTimestamp(DateTime.Now.AddDays(-42)));

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    activities = JsonConvert.DeserializeObject<List<Activity>>(response.Content);
                    return activities;
                }
                catch (Exception ex)
                {
                    return activities;
                }
            }
            else
            {
                return activities;
            }
        }

        public async Task<MyFitness.Model.Strava.Stream[]> GetActivityStream(int activityId, StreamType type)
        {
            Stream[] s = new Stream[] { new Stream() };          

            FitnessResponse response = await _webService.ReceiveRequest(
                "https://www.strava.com/api/v3/activities/"
                + activityId + "/streams/"
                + Enum.GetName(typeof(StreamType), type)
                + "?access_token="
                + Settings.AccessToken);

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    s = JsonConvert.DeserializeObject<Stream[]>(response.Content);
                    return s;
                }
                catch (Exception ex)
                {
                    return s;
                }
            }
            else
            {
                return s;
            }

        }

        public async Task<ActivityZones> GetActivityZones()
        {
            var zones = new ActivityZones();

            FitnessResponse response = await _webService.ReceiveRequest(
                "https://www.strava.com/api/v3/athlete/zones?access_token="
                + Settings.AccessToken);

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    zones = JsonConvert.DeserializeObject<ActivityZones>(response.Content);
                    return zones;
                }
                catch (Exception ex)
                {
                    return zones;
                }
            }
            else
            {
                return zones;
            }

        }

        public async Task<Athlete> GetAthlete()
        {
            var athlete = new Athlete();

            FitnessResponse response = await _webService.ReceiveRequest("https://www.strava.com/api/v3/athlete?access_token=" + Settings.AccessToken);

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    athlete = JsonConvert.DeserializeObject<Athlete>(response.Content);
                    return athlete;
                }
                catch (Exception ex)
                {
                    return athlete;
                }
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
