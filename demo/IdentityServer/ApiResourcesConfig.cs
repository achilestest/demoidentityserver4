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
            foreach (Route item in Route.ListRoute())
            {
                result.Add(new ApiResource(item.Name,item.Url));
            }
            return result;
        }
    }
}
