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
        #region Constructors

        public App()
        {
            if (IsAuthenticated)
            {
                MainPage = new Main(this);
            }
            else
            {
                if (!Settings.HasCompletedInitialSetup)
                {
                    MainPage = new Setup(this);
                }
                else
                {
                    Logout();
                }
            }

            if (_Instance == null)
            {
                _Instance = this;
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

        #endregion

        #region Properties

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

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(Settings.AccessToken); }
            set { Settings.AccessToken = null; }
        }

        public string Token
        {
            get { return Helpers.Settings.AccessToken; }
        }

        #endregion

        #region Methods
        public void SaveToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                Settings.AccessToken = token;
            }       
        }

        public void ShowMainPage()
        {
            MainPage = new LoginModalPage(this);
        }

        public void Logout()
        {
            Settings.AccessToken = "";

            if (Device.OS == TargetPlatform.Android)
            {
                MainPage = new Main(this);
                MainPage.Navigation.PushModalAsync(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        public void ReturnFromLogin()
        {
            MainPage = new Main(this);
        }

        public Action SuccessfulLoginAction
        {
            get
            {
                return new Action(ReturnFromLogin);
            }
        }

        public void SetupColors()
        {
            Settings.BackgroundColor = "BEDEE8";
            Settings.FontColor = "251D57";
        }

        #endregion
    }
}
