using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo.IdentityServer
{
    public class Discovery
    {
        public static string LoginEndpoint { get { return Config.Configuration["BaseUrl"]+"connect/token"; } }
        public static string RefreshTokenEndpoint { get { return Config.Configuration["BaseUrl"] + "connect/token"; } }
    }
}
