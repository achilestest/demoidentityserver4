using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo
{
    public class EndPoint
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public static List<EndPoint> ListRoute()
        {
            List<EndPoint> result = new List<EndPoint>
            {
                new EndPoint{Name="GetUser",Url="api/users/get",Controller="Users", Action="GetUsers"},
                new EndPoint{Name="Login",Url="api/users/login",Controller="Users", Action="Login"},
                new EndPoint{Name="Test",Url="api/values",Controller="Values", Action="Get"},
            };
            return result;
        }
    }
}
