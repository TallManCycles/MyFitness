using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MyFitness.Pages
{
    public class MainContentPage : BaseContentPage
    {
        public MainContentPage()
        {
            MessagingCenter.Subscribe<App>(this, "Authenticated", (sender) => {
                Content = new StackLayout
                {
                    Children = {
                    new Label { Text = "Hello ContentPage" }
                }
                };
            });

            
        }
    }
}
