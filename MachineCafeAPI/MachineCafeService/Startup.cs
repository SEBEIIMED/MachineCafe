using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineCafe.Core.Data;
using MachineCafe.Core.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MachineCafe.Core.Data;
using MachineCafe.Core.Repository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MachineCafe.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Machine Cafe",
                    Description = "Machine cafe Test",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "IMED SEBEI", Email = "imed.sebei@gmail.com", Url = "www.machine_cafe.com" }
                });                
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
               
            });
            services.AddDbContext<DataContext>(options =>
       options.UseSqlServer(@"Server=localhost;Database=MachineCafe;Trusted_Connection=False;MultipleActiveResultSets=true;User Id=sa;Password=adminsebei;"));
            services.AddTransient<IMachineCafeChoixRepository, MachineCafeChoixRepository>();  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Machine Cafe API");
            });
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));

            //var tokenProviderOptions = new TokenProviderOptions
            //{
            //    Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
            //    Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
            //    Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
            //    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            //    IdentityResolver = GetIdentity
            //};

            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    // The signing key must match!
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = signingKey,
            //    // Validate the JWT Issuer (iss) claim
            //    ValidateIssuer = true,
            //    ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
            //    // Validate the JWT Audience (aud) claim
            //    ValidateAudience = true,
            //    ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
            //    // Validate the token expiry
            //    ValidateLifetime = true,
            //    // If you want to allow a certain amount of clock drift, set that here:
            //    ClockSkew = TimeSpan.Zero
            //};

            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = tokenValidationParameters
            //});
            //var sss = new CookieAuthenticationOptions();
            //app.UseCookieAuthentication(sss);
            
           
        }

    }

}