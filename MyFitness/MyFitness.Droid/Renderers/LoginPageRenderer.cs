using System;
using Android.App;
using Xamarin.Auth;
using Xamarin.Forms;
using MyFitness.Pages;
using MyFitness;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform;
using MyFitness.Droid;
using MyFitness.Services;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace MyFitness.Droid
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // this is a ViewGroup - so should be able to load an AXML file and FindView<>
            var activity = this.Context as Activity;

            var auth = new OAuth2Authenticator(
               clientId: App.Instance.OAuthSettings.ClientId,
               clientSecret: App.Instance.OAuthSettings.ClientSecret,
               authorizeUrl: new Uri(App.Instance.OAuthSettings.AuthorizeUrl), 
               scope: App.Instance.OAuthSettings.Scope, 
               redirectUrl: new Uri(App.Instance.OAuthSettings.RedirectUrl),
               accessTokenUrl: new Uri(App.Instance.OAuthSettings.AccessToken), 
               getUsernameAsync: null); 

            auth.Completed += (sender, eventArgs) => {
                if (eventArgs.IsAuthenticated)
                {
                    App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);
                    App.Instance.SuccessfulLoginAction.Invoke();
                    MessagingCenter.Send<ContentPage>(new ContentPage(), "Login");
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
    }
}
