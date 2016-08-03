using Xamarin.Forms;

namespace MyFitness.Pages
{
    public class BaseContentPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!App.Instance.IsAuthenticated)
            {
                Navigation.PushModalAsync(new LoginPage());
            }
        }
    }
}

