using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Resources;
using IdentityServer3.Core.Services;
using IdentityServer3.Ws_Federation.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using Serilog;
using Scopes = IdentityServer3.Ws_Federation.Configuration.Scopes;

[assembly: OwinStartup(typeof(IdentityServer3.Ws_Federation.Startup))]


namespace IdentityServer3.Ws_Federation
{
    public class Startup
    {

        public void Configuration(IAppBuilder appBuilder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({Name}){NewLine} {Message}{NewLine}{Exception}")
                .CreateLogger();

            var factory = new IdentityServerServiceFactory()
                .UseInMemoryClients(Clients.Get())
                .UseInMemoryScopes(Scopes.Get());
            factory.UserService = new Registration<IUserService>(typeof(ExternalRegistrationUserService));

            var options = new IdentityServerOptions
            {
                SigningCertificate = Certificate.Load(),
                Factory = factory,
                AuthenticationOptions = new AuthenticationOptions
                {
                    EnableLocalLogin = false,
                    IdentityProviders = ConfigureIdentityProviders
                }
            };

            appBuilder.UseIdentityServer(options);
        }

        private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var wsFederation = new WsFederationAuthenticationOptions
            {
                AuthenticationType = "windows",
                Caption = "Windows",
                SignInAsAuthenticationType = signInAsType,

                MetadataAddress = "https://localhost:44366",
                Wtrealm = "urn:idsrv3"
            };
            app.UseWsFederationAuthentication(wsFederation);
        }

    }
}