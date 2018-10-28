using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Configuration.Overrides;
using Orleans.Hosting;
using Orleans.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rhendaria.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(ConfigureSwagger);
            services.AddSingleton(CreateClusteClientInstance);
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rhendaria Api");
                options.OAuthClientId("swaggerui");
                options.OAuthAppName("Swagger UI");
            });

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void ConfigureSwagger(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Info { Title = "Rhendaria Api", Version = "v1" });
        }

        private static IClusterClient CreateClusteClientInstance(IServiceProvider services)
        {
            IClientBuilder clientBuilder = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "rhendaria-cluster";
                    options.ServiceId = "rhendaria-game-engine";
                })
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=orleans;Pooling=true;";
                    options.Invariant = "Npgsql";
                })
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Information).AddFile("OrleansWebClient.log"));

            IClusterClient client = clientBuilder.Build();

            Task.WaitAll(client.Connect());

            return client;
        }
    }
}
