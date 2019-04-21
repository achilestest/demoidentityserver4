using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo.IdentityServer
{
    public partial class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            List<ApiResource> result = new List<ApiResource>();
            result.Add(new ApiResource("api", "My API Set #1", new[] { JwtClaimTypes.Name, JwtClaimTypes.Role, "office" }));
            return result;
        }
    }
}
