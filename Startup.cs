using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using OdeToFood.Services;
using Microsoft.AspNetCore.Routing;
using OdeToFood.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OdeToFood
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        //public Startup(IHostingEnvironment env, ApplicationEnvironment appEnv)
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())//have to set base path with current version@
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddEntityFrameworkSqlServer().AddDbContext<OdeToFoodDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OdeToFoodDatabase")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<OdeToFoodDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton(povider => Configuration);
            services.AddSingleton<IGreeter, Greeter>();
            services.AddScoped<IRestaurantData, SqlResaurantData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IGreeter greeter, IHostingEnvironment appEnvironment)
        {
            //app.UseIISPlatformHandler(); this is now integrated into the framework

            app.UseDeveloperExceptionPage();
            //app.UseRuntimeInfoPage("/info");

            
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            app.UseFileServer(); //combines the 2 above
            app.UseNodeModules(appEnvironment);
            app.UseIdentity();
            app.UseMvc(ConfigureRoutes);

            //app.Run is like a "terminal" peice of middleware. no middleware written after this will run
            app.Run(async (context) =>
            {
                //throw new Exception("Error!");

                var greeting = greeter.GetGreeting();
                await context.Response.WriteAsync(greeting);
            });
        }

        //convention based routing
        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            // /
            // /Home/Index -- you want to reach the home controller and call the Index method

            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
