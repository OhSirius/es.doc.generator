using BG.Domain.Authentication.WCFServices.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace BG.Domain.Authentication.Setup
{
    public static class Installer
    {
        public static RouteBase CreateAuthenticationRoute(string url)
        {
            return new ServiceRoute(url, new AuthenticationServiceHostFactory(), typeof(AuthenticationService));
        }
    }
}
