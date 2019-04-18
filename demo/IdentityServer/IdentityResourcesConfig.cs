using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo.IdentityServer
{
    public partial class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentities()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }
    }
}
