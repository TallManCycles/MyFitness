using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyFitness.Services
{
    class WebService
    {
        public WebService()
        {
        }

        public async Task<WebResponse> Get(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                WebResponse result = await request.GetResponseAsync();
                if (result.ContentLength > 0)
                {
                    Stream stream = result.GetResponseStream();
                }
                return result;
            }
            catch (WebException e)
            {
                throw;
            }
        }
    }
}
