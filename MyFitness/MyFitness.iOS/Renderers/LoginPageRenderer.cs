using MyFitness.iOS.Renderers;
using MyFitness.Pages;
using System;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace MyFitness.iOS.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        bool IsShown;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!IsShown)
            {
                IsShown = true;

                var auth = new OAuth2Authenticator(
               clientId: App.Instance.OAuthSettings.ClientId,
               clientSecret: App.Instance.OAuthSettings.ClientSecret,
               authorizeUrl: new Uri(App.Instance.OAuthSettings.AuthorizeUrl),
               scope: App.Instance.OAuthSettings.Scope,
               redirectUrl: new Uri(App.Instance.OAuthSettings.RedirectUrl),
               accessTokenUrl: new Uri(App.Instance.OAuthSettings.AccessToken),
               getUsernameAsync: null);

                auth.Error += (object sender, AuthenticatorErrorEventArgs eventArgs) => {
                    auth.ShowUIErrors = false;
                    auth.AllowCancel = true;
                };

                auth.BrowsingCompleted += (sender, eventargs) =>
                {
                    var s = eventargs;
                };

                auth.Completed += (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        App.Instance.SuccessfulLoginAction.Invoke();
                        App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);

                        DismissViewController(true, null);
                    }
                    else
                    {
                        System.Diagnostics.Debug.Write("Fail");
                    }
                };

                PresentViewController(auth.GetUI(), false, null);
            }
        }
    }
}