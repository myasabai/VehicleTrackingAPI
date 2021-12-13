using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VehicleAPI.Authentication;
using VehicleAPI.Interfaces;
using VehicleAPI.Managers;
using VehicleAPI.Models;

namespace VehicleAPI
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
            services.AddControllers();
            services.AddDbContext<VehicleTrackingContext>(
        options => options.UseSqlServer("Data Source=DESKTOP-RLJ65LJ;Initial Catalog=VehicleTracking;User Id=sa;Password=msbsql2020update"));
            //services.AddScoped<IRepository,BaseRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            //services.AddTransient(typeof(IRepository<Location>), typeof(BaseRepository<Location>));
            //services.AddTransient(typeof(IRepository<User>), typeof(BaseRepository<User>));
            //services.AddTransient(typeof(IRepository<Device>), typeof(BaseRepository<Device>));
            services.AddAuthentication("BasicAuthentication")
   .AddScheme<AuthenticationSchemeOptions, BasicAuthentication>("BasicAuthentication", null); //Custom Added
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication(); //Custom Added

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
