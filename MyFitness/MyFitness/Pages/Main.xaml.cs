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
            Detail = new NavigationPage(new MainPage());

            masterPage.ListView.ItemSelected += OnItemSelected;
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
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    masterPage.ListView.SelectedItem = null;
                    IsPresented = false;
                }                
            }
        }
    }
}
