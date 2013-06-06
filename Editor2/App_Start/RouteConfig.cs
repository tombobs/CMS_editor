using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Editor2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("EditorApi.asmx");
            // operations
            routes.IgnoreRoute("EditorApi.asmx/DocCreate");
            routes.IgnoreRoute("EditorApi.asmx/DocDelete");
            routes.IgnoreRoute("EditorApi.asmx/ElementCreate");
            routes.IgnoreRoute("EditorApi.asmx/ElementDelete");
            routes.IgnoreRoute("EditorApi.asmx/ElementEdit");
            routes.IgnoreRoute("EditorApi.asmx/ElementMove");
            //utils
            routes.IgnoreRoute("EditorApi.asmx/GetNumElementsInDoc");
            routes.IgnoreRoute("EditorApi.asmx/GetDocXml");
            routes.IgnoreRoute("EditorApi.asmx/GetDocHtml");            
            // testing
            routes.IgnoreRoute("EditorApi.asmx/TestHtml");
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}