using System;
using Android.App;
using Xamarin.Auth;
using Xamarin.Forms;
using MyFitness.Pages;
using MyFitness;
using Xamarin.Forms.Platform.Android;
using OAuthTwoDemo.XForms.Android;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace OAuthTwoDemo.XForms.Android
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
               clientSecret: App.Instance.OAuthSettings.ClientSecret,// your OAuth2 client id
               authorizeUrl: new Uri(App.Instance.OAuthSettings.AuthorizeUrl), // the auth URL for the service
               scope: App.Instance.OAuthSettings.Scope, // The scopes for the particular API you're accessing. The format for this will vary by API.
               redirectUrl: new Uri(App.Instance.OAuthSettings.RedirectUrl),
               accessTokenUrl: new Uri(App.Instance.OAuthSettings.AccessToken), 
               getUsernameAsync: null); // the redirect URL for the service

            auth.Completed += (sender, eventArgs) => {
                if (eventArgs.IsAuthenticated)
                {
                    App.Instance.SuccessfulLoginAction.Invoke();
                    // Use eventArgs.Account to do wonderful things
                    App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);
                }
                else
                {
                    //;
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
    }
}