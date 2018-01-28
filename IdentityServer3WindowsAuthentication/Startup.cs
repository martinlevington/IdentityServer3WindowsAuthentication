using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer.WindowsAuthentication.Configuration;
using IdentityServer3WindowsAuthentication.Configuration;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IdentityServer3WindowsAuthentication.Startup))]

namespace IdentityServer3WindowsAuthentication
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            app.UseWindowsAuthenticationService(new WindowsAuthenticationOptions
            {
                IdpReplyUrl = "https://localhost:44377/was",
                SigningCertificate = Certificate.Load(),
                EnableOAuth2Endpoint = false
            });
        }
    }
}