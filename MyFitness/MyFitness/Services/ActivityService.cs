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

        public async Task<FitnessResponse> GetAthleteActivities(string authenticationToken)
        {
            var response = new FitnessResponse();

            response = await _webService.ReceiveRequest("https://www.strava.com/api/v3/athlete/activities?include_all_efforts=true&access_token=" + authenticationToken);

            if (response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    var Activity = JsonConvert.DeserializeObject<List<Activity>>(response.Content);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return response;
        }

        
    }
}
