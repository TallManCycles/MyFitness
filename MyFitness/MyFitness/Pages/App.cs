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
                                    clientSecret: "ea5255107b2661d167cc46d1d42b4c8dbe43d318");
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
                if (Settings.HasCompletedInitialSetup)
                {
                    _NavPage = new NavigationPage(new MainPage());
                }
            }
            else
            {
                if (Settings.HasCompletedInitialSetup)
                {
                    _NavPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    _NavPage = new NavigationPage(new InitialSetup(this)); 
                }
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
            MainPage = GetMainPage();
        }

        public void Logout()
        {
            Settings.AccessToken = "";
            _NavPage.Navigation.PopModalAsync();
            MainPage = GetMainPage();
        }

        public void Initialize()
        {
            MainPage = new NavigationPage(new InitialSetup(DependencyService.Get<ILoginManager>()));
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
