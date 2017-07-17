using System.Web.Mvc;
using System.Web.Routing;

namespace Template.MVCApp.Engine
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection Routes)
        {
            Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Access", action = "Index", id = UrlParameter.Optional },
                new[] { "Template.MVCApp.Controllers" }
                );
        }
    }
}
