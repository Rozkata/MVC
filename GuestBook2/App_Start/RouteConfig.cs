using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GuestBook2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CommentsByDate",
                url: "Guestbook/CommentsByDate/{userDate}",
                defaults: new { controller = "Guestbook", action = "CommentsByDate", userDate = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Comments",
                url: "Guestbook/Comments/{userName}",
                defaults: new { controller = "Guestbook", action = "Comments", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
