using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Pineapple.Database;
using Pineapple.Service.Filters;
using Pineapple.Service.Infrastructure.Authorization;
using Swashbuckle.AspNetCore.Swagger;

namespace Pineapple.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                //swaggerGenOptions.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            services.ConfigureSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            var builder = new ContainerBuilder();
            Register(builder);
            builder.Populate(services);
            Container = builder.Build();
            return new AutofacServiceProvider(Container);
        }

        private void Register(ContainerBuilder builder)
        {
            builder.Register<IConfigurationRoot>(x => Configuration).SingleInstance();

            builder.Register<TokenProviderOptions>(x =>
            {
                var config = x.Resolve<IConfigurationRoot>();
                String secretKey = config.GetSection("JwtSecretKey").Value;
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
                var options = new TokenProviderOptions
                {
                    Path = "/token",
                    Expiration = TimeSpan.FromDays(1),
                    Audience = "ExampleAudience",
                    Issuer = "ExampleIssuer",
                    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                };
                return options;
            }).SingleInstance();

            builder.Register<PineappleContext>(x =>
            {
                var config = x.Resolve<IConfigurationRoot>();
                var envName = config["environment"];
                var connectionString = config
                    .GetConnectionString(envName);
                return new PineappleContext(connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ConfigureJwt(app);

            app.UseMvc();
            loggerFactory.AddConsole();

            app.UseMvc();

            ConfigureSwagger(app);
        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
        }

        private void ConfigureJwt(IApplicationBuilder app)
        {
            var options = Container.Resolve<TokenProviderOptions>();

            //app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.SigningCredentials.Key,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ExampleIssuer",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "ExampleAudience",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
        }

        public IContainer Container { get; set; }
    }
}
