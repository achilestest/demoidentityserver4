using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using demo.IdentityServer;
using demo.Models;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace demo
{
    public class Startup
    {
        IHostingEnvironment Environment;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication()
                .AddIdentityServerAuthentication("token", isAuth =>
                {
                    isAuth.Authority = "http://localhost:17039/";
                    isAuth.ApiName = "api";
                    isAuth.RequireHttpsMetadata = false;
                });
            string connection = Configuration["ConnectionStrings:BusinessConnection"];
            services.AddDbContext<BusinessContext>(options => options.UseSqlServer(connection));

            // Identity Server 4
            //X509Certificate2 cert = new X509Certificate2(Path.Combine(Environment.ContentRootPath, "idsrv4test.pfx"), "123456");
            services.AddIdentityServer()
            //    .AddSigningCredential(cert)
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentities())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddProfileService<ProfileService>();

            //Inject the classes we just created
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();

            // Mapping Model and ModelView
            Mapper.Initialize(cfg => { Mapping.MappingBuilder(cfg);});

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.UseMvc(routes => RouteConfig.RouteBulder(routes));
        }
    }
}
