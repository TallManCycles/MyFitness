using MyFitness.Authentication;
using MyFitness.Helpers;
using MyFitness.Pages;
using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MyFitness
{
    public class App : Application, ILoginManager
    {
        public App()
        {
            MainPage = GetMainPage();
        }

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
                                    clientId: "1255",
                                    scope: "write", 
                                    authorizeUrl: "https://www.strava.com/oauth/authorize", 
                                    redirectUrl: "http://www.tallmancycles.com.au",
                                    accessToken: "https://www.strava.com/oauth/token",
                                    clientSecret: "");
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
            if (IsAuthenticated)
            {
                _NavPage = new NavigationPage(new MainPage());
            }
            else
            {
                _NavPage = new NavigationPage(new LoginPage());
            }

            return _NavPage;
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(Settings.AccessToken); }
        }

        public string Token
        {
            get { return Helpers.Settings.AccessToken; }
        }

        public void SaveToken(string token)
        {
            Settings.AccessToken = token;
            MainPage = new NavigationPage(new MainPage());
        }

        public void Logout()
        {
            Settings.AccessToken = "";
            MainPage = new NavigationPage(new LoginPage());
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
