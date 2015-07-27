using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BCBS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Project", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
          "Default2", // Route name
          "{controller}/{contractid}/{action}/{activityid}", // URL with parameters
          new { controller = " Contract", contractId = UrlParameter.Optional, action = "NewActivity", activityid = UrlParameter.Optional } // Parameter defaults
      );
        }
    }
}