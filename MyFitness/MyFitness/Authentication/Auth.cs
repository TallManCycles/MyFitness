using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Authentication
{
    public class Auth
    {
        public Auth(
            string clientId,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessToken,
            string clientSecret)
        {
            ClientId = clientId;
            Scope = scope;
            AuthorizeUrl = authorizeUrl;
            RedirectUrl = redirectUrl;
            ClientSecret = clientSecret;
            AccessToken = accessToken;
        }

        public string ClientId { get; private set; }
        public string Scope { get; private set; }
        public string AuthorizeUrl { get; private set; }
        public string RedirectUrl { get; private set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
    }
}
