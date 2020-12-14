using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OpenDiscussionPlatform
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "ShowPostari",
               url: "postari/show/{id}",
               defaults: new { controller = "ForumPosts", action = "Show", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "DefaultPostari",
                url: "postari/{action}/{id}",
                defaults: new { controller = "ForumPosts", action = "listare", id = UrlParameter.Optional }
            );
        



        routes.MapRoute(
                name: "Search",
                url: "search/{keyword}",
                defaults: new { controller = "Search", action = "SearchSequence", keyword = UrlParameter.Optional }
                );


            // D E F A U L T  
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
