using MyFitness.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Service
{
    public class WebService
    {
        public async Task<FitnessResponse> ReceiveRequest(string url)
        {
            var response = new FitnessResponse();

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "GET";           

            try
            {
                using (WebResponse httpResponse = await httpRequest.GetResponseAsync())
                {
                    if (httpResponse != null)
                    {
                        response.Status = ((HttpWebResponse)httpResponse).StatusCode;
                        Stream stream = httpResponse.GetResponseStream();
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            response.Content = await streamReader.ReadToEndAsync();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (WebResponse httpResponse = ex.Response)
                    {
                        response.Status = ((HttpWebResponse)httpResponse).StatusCode;

                        Stream stream = httpResponse.GetResponseStream();

                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            response.Content = await streamReader.ReadToEndAsync();
                        }
                    }
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    return response;
                }
            }

            return response;
        }
    }
}