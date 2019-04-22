using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
namespace demo
{
    public class RouteConfig
    {

        public static IRouteBuilder RouteBulder(IRouteBuilder builder)
        {
            List<EndPoint> routes = EndPoint.ListRoute();
            foreach(EndPoint item in routes)
            {
                builder.MapRoute(item.Name, item.Url, defaults: new { controller = item.Controller, action = item.Action });
            }

            return builder;
        }
    }
}
