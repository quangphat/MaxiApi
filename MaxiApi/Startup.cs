using Maxi.Models.Infrastructures;
using Maxi.Repository;
using Maxi.Repository.Classes;
using Maxi.Repository.Interfaces;
using MaxiApi.Infrastructures;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MaxiApi
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
            services.AddControllers()
               .AddNewtonsoftJson(option =>
               {
                   option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
               });
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<MaxiCorpContext>(option =>
                {
                    var s = Configuration["ConnectionString"];
                    option.UseSqlServer(Configuration["ConnectionString"]
                        , sqlOption => sqlOption.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
                });
            services.AddScoped<CurrentProcess>();
            services.RegisterRepository();
            services.RegisterBusiness();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
