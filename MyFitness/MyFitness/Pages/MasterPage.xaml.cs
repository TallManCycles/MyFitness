using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public partial class MasterPage : ContentPage
    {
        private ILoginManager _loginManager;

        public ListView ListView { get { return listView; } }

        public MasterPage(ILoginManager loginManager)
        {
            _loginManager = loginManager;
            InitializeComponent();

            this.Title = "Main Page";

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "My Fitness",
                TargetType = typeof(MainPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Logout"
            });

            listView.ItemsSource = masterPageItems;
        }
    }

    public class MasterPageItem
    {
        public string Title { get; set; }

        public ImageSource IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}
