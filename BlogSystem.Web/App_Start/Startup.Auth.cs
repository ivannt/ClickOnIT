namespace BlogSystem.Web
{
    using System;

    using BlogSystem.Data;
    using BlogSystem.Data.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Uncomment the following lines to enable logging in with third party login providers
            //// app.UseMicrosoftAccountAuthentication(
            ////     clientId: "",
            ////     clientSecret: "");

            //// app.UseTwitterAuthentication(
            ////    consumerKey: "",
            ////    consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: "1090666267610247",
               appSecret: "43e52ccbe2e3016b47c964462da7d394");

            app.UseGoogleAuthentication(
                clientId: "312180262270-6hvrs5trann015q4gdjvnks5boqebiqc.apps.googleusercontent.com",
                clientSecret: "qfQIlFk7oPDz6RZmpphuhQuw");
        }
    }
}