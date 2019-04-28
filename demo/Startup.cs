using System.Security.Cryptography;
using AutoMapper;
using demo.IdentityServer;
using demo.Models;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace demo
{
    public class Startup
    {
        IHostingEnvironment Environment;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Connect to Database
            string connection = Configuration["ConnectionStrings:BusinessConnection"];
            services.AddDbContext<BusinessContext>(options => options.UseSqlServer(connection));

            // Identity Server 4
            var _rsa = new RSACryptoServiceProvider(2048);
            SigningCredentials cer = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256Signature);
            services.AddIdentityServer(IdentityServerConfig.SetupIdentityServer)
                .AddSigningCredential(cer)
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentities())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddProfileService<ProfileService>();
         
           var authUrl = Configuration["BaseUrl"];
            //Custom login
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.Authority = authUrl;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = authUrl,
                     ValidateAudience = false,
                     ValidAudience = "api",
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
            loggerFactory.AddDebug(LogLevel.Trace); // you're not passing the LogLevel!
            //app.Map("/api", authApp =>
            //{
            //    app.UseStaticFiles("/api");
            //    authApp.UsePathBase(new PathString("/api"));
            //    authApp.UseIdentityServer();
            //    authApp.UseAuthentication();
            //    authApp.UseMvc(routes => RouteConfig.RouteBulder(routes));

            //});
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc(routes => RouteConfig.RouteBulder(routes));
        }
    }
}
