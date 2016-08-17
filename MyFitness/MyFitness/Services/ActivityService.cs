using MyFitness.Model;
using MyFitness.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyFitness.Model.Strava;

namespace MyFitness.Services
{
    public class ActivityService
    {
        private WebService _webService;

        public ActivityService()
        {
            _webService = new WebService();
        }

        public async Task<List<Activity>> GetAthleteActivities(string authenticationToken)
        {
            FitnessResponse response = await _webService.ReceiveRequest(
                "https://www.strava.com/api/v3/athlete/activities?include_all_efforts=true&access_token=" 
                + authenticationToken 
                + "&after=" 
                + ConvertToUnixTimestamp(DateTime.Now.AddDays(-42)));

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Activity>>(response.Content);
                }
                catch (Exception ex)
                {
                    return new List<Activity>();
                }
            }
            else
            {
                return new List<Activity>();
            }
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
