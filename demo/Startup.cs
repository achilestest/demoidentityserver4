using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using demo.IdentityServer;
using demo.Models;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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
            IdentityModelEventSource.ShowPII = true;
            string connection = Configuration["ConnectionStrings:BusinessConnection"];
            services.AddDbContext<BusinessContext>(options => options.UseSqlServer(connection));

            // Identity Server 4
            var _rsa = new RSACryptoServiceProvider(2048);
            SigningCredentials cer = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256Signature);
            services.AddIdentityServer()
                .AddSigningCredential(cer)
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentities())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddProfileService<ProfileService>();
            var authUrl = "http://localhost:17039";
            //Inject the classes we just created
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.Authority = authUrl;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = authUrl,
                     ValidateAudience = false,
                     ValidAudience = "sinanbir.com.auth",//test purposes
                     ValidateLifetime = true,
                 };
             });
            // Mapping Model and ModelView
            Mapper.Initialize(cfg => { Mapping.MappingBuilder(cfg);});

            
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Trace); // ⇦ you're not passing the LogLevel!
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc(routes => RouteConfig.RouteBulder(routes));
        }
    }
}
