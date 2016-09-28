using MyFitness.Calculations;
using MyFitness.Data;
using MyFitness.Helpers;
using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class Main : MasterDetailPage
    {
        MasterPage masterPage;
        private ILoginManager _loginManager;

        public Main(ILoginManager loginManager)
        {
            _loginManager = loginManager;
            masterPage = new MasterPage(_loginManager);
            Master = masterPage;
            try
            {
                Detail = new NavigationPage(new MainPage(DependencyService.Get<ActivityService>(),
                                                        DependencyService.Get<Sql>()))
                {
                    BarBackgroundColor = Color.FromHex(Settings.BackgroundColor),
                    BarTextColor = Color.FromHex(Settings.FontColor)
                };
            }
            catch (Exception ex)
            {

                throw;
            }
            

            masterPage.ListView.ItemSelected += OnItemSelected;

            this.BackgroundColor = Color.FromHex(Settings.BackgroundColor);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                if (item.Title == "Logout")
                {
                    _loginManager.Logout();
                }
                else if (item.Title == "Dashboard")
                {
                    Detail = new NavigationPage(new MainPage(   DependencyService.Get<ActivityService>(),
                                                                DependencyService.Get<Sql>()))
                    {
                        BarBackgroundColor = Color.FromHex(Settings.BackgroundColor),
                        BarTextColor = Color.FromHex(Settings.FontColor)
                    };
                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                    {
                        BarBackgroundColor = Color.FromHex(Settings.BackgroundColor),
                        BarTextColor = Color.FromHex(Settings.FontColor)
                    };
                };
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
