﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo
{
    public class Route
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public static List<Route> ListRoute()
        {
            List<Route> result = new List<Route>
            {
                new Route{Name="GetUser",Url="api/users/get",Controller="Users", Action="GetUsers"},
                new Route{Name="Login",Url="api/users/login",Controller="Users", Action="Login"},
            };
            return result;
        }
    }
}
