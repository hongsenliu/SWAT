using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SWAT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "CountriesList",
                 "Location/Countries/List/{regionID}",
                 new { controller = "Location", action = "CountryList", regionID = UrlParameter.Optional }
            );

            routes.MapRoute(
                 "SubnationsList",
                 "Location/Subnations/List/{countryID}",
                 new { controller = "Location", action = "SubnationList", countryID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Details", id = 191 }
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}