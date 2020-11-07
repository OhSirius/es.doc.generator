using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BG.Domain.Authentication.Setup;

namespace BG.Application.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //Устанавливаем сервис аутентификации
            routes.Add(Installer.CreateAuthenticationRoute("BGAuth"));//метаданные сервиса http://localhost:52740/BGAuth?wsdl

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
