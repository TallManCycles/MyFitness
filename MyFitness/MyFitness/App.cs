using MyFitness.Authentication;
using MyFitness.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MyFitness
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = GetMainPage();
        }

        // just a singleton pattern so I can have the concept of an app instance
        static volatile App _Instance;
        static object _SyncRoot = new Object();
        public static App Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new App();
                            _Instance.OAuthSettings =
                                new Auth(
                                    clientId: "1255",       // your OAuth2 client id 
                                    scope: "write",  		// The scopes for the particular API you're accessing. The format for this will vary by API.
                                    authorizeUrl: "https://www.strava.com/oauth/authorize",  	// the auth URL for the service
                                    redirectUrl: "http://localhost/",
                                    accessToken: "95a4dbb088a2453660c61e7b0f06df7bc9d7f792",
                                    clientSecret: "ea5255107b2661d167cc46d1d42b4c8dbe43d318");   // the redirect URL for the service
                        }
                    }
                }

                return _Instance;
            }
        }

        public Auth OAuthSettings { get; private set; }

        NavigationPage _NavPage;

        public Page GetMainPage()
        {
            var profilePage = new ProfilePage();

            _NavPage = new NavigationPage(profilePage);

            return _NavPage;
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(_Token); }
        }

        string _Token;
        public string Token
        {
            get { return _Token; }
        }

        public void SaveToken(string token)
        {
            _Token = token;

            // broadcast a message that authentication was successful
            MessagingCenter.Send<App>(this, "Authenticated");
        }

        public Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() => _NavPage.Navigation.PopModalAsync());
            }
        }
    }
}
