using MyFitness.iOS.Renderers;
using MyFitness.Pages;
using System;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace MyFitness.iOS.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        bool IsShown = false;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            var auth = new OAuth2Authenticator(
           clientId: App.Instance.OAuthSettings.ClientId,
           clientSecret: App.Instance.OAuthSettings.ClientSecret,
           scope: App.Instance.OAuthSettings.Scope,
           authorizeUrl: new Uri(App.Instance.OAuthSettings.AuthorizeUrl),
           redirectUrl: new Uri("http://www.tallmancycles.com.au"),
           accessTokenUrl: new Uri(App.Instance.OAuthSettings.AccessToken)
           );

            auth.AllowCancel = true;

            auth.Completed += (sender, eventArgs) =>
            {
                DismissViewController(true, null);

                if (eventArgs.IsAuthenticated && auth.HasCompleted)
                {
                    App.Instance.SuccessfulLoginAction.Invoke();
                    App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);
                    IsShown = true;
                    return;                   
                }
                else
                {
                    System.Diagnostics.Debug.Write("Fail");
                }
            };

            if (!IsShown)
                PresentViewController(auth.GetUI(), false, new Action(() => finalisedAction()));
        }

        private void Auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            System.Diagnostics.Debug.Write("Fail");
        }

        public void finalisedAction()
        {
            System.Diagnostics.Debug.Write("Fail");
        }
    }
}