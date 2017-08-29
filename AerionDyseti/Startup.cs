using AerionDyseti.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Text;

namespace AerionDyseti
{
    public class Startup
    {
        // Properties
        public IConfiguration Configuration { get; }


        // Ctor
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable injection of JwtSettings into Controllers by setting up a JwtSettings Service.
            services.Configure<JwtSettings>(jwtSettings =>
            {
                jwtSettings.Issuer = Configuration["JwtSettings:Issuer"];
                jwtSettings.Audience = Configuration["JwtSettings:Audience"];
                jwtSettings.Secret = Configuration["JwtSettings:Secret"];
            });

            // Add Identity, using the given password requirements.
            services.AddIdentity<AerionDysetiUser, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 6,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };

                options.User = new UserOptions
                {
                    RequireUniqueEmail = true
                };

            })
            // Setup Identity to use EF for its stores.
            .AddEntityFrameworkStores<AerionDysetiContext>();


            // Configure all of the JWT Authentication services.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Use JwtBearer Authentication with the given TokenValidation Parameters.
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!  
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSettings:Secret"])),

                // Validate the JWT Issuer (iss) claim  
                ValidateIssuer = true,
                ValidIssuer = Configuration["JwtSettings:Issuer"],

                // Validate the JWT Audience (aud) claim  
                ValidateAudience = true,
                ValidAudience = Configuration["JwtSettings:Audience"],

                // Validate the token expiry  
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            });


            // Setup MVC to Indented JSON output and ignore null values when serializing.
            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.SerializerSettings.Formatting = Formatting.Indented;
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            // Add EF Context Service.
            services.AddDbContext<AerionDysetiContext>(options => options.UseMySql(Configuration["dbConnectionString"]));

        }



        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }




    }
}
