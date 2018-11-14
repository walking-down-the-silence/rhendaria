using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Rhendaria.Web.Hubs;
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
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/gameHub");
            });
        }

        private static void ConfigureSwagger(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Info { Title = "Rhendaria Api", Version = "v1" });
        }

        private static IClusterClient CreateClusteClientInstance(IServiceProvider services)
        {
            IClientBuilder clientBuilder = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "rhendaria.server.cluster";
                    options.ServiceId = "rhendaria.server.service";
                });

            IClusterClient client = clientBuilder.Build();

            Task.WaitAll(client.Connect());

            return client;
        }
    }
}
