using IdentityServer4.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo.IdentityServer
{
    public partial class IdentityServerConfig
    {
        public static void SetupIdentityServer(IdentityServerOptions identityServerOptions)
        {
            //identityServerOptions.UserInteraction.LoginUrl = "/api/users/login";
            //identityServerOptions.Endpoints.EnableDiscoveryEndpoint = false;
            //identityServerOptions.Endpoints.EnableUserInfoEndpoint = false;
            //identityServerOptions.Endpoints.EnableTokenEndpoint = false;
        }
    }
}
