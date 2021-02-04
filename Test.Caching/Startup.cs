using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy.Core.Flow.Caching;
using Easy.Core.Flow.Caching.Configuration;
using Easy.Core.Flow.Caching.Memory;
using Microsoft.AspNetCore.Http;

namespace Test.Caching
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test.Caching", Version = "v1" });
            });

            services.AddEasyCoreFlowCaching(configurationAction =>
            {
                var defaultSlidingExpireTime = TimeSpan.FromHours(24);
                configurationAction.ConfigureAll(cache =>
                {
                    cache.DefaultSlidingExpireTime = defaultSlidingExpireTime;
                });
                return configurationAction;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test.Caching v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.Run(async context =>
            {
                var test1 = app.ApplicationServices.GetService<ICacheManager>().GetAllCaches();

                app.ApplicationServices.GetService<ICacheManager>().GetCache("TestDefaultCache").Set("1","AAAAAAAAAAAAAAAAAAA");

                var test2 = app.ApplicationServices.GetService<ICacheManager>().GetCache("TestDefaultCache").Get("1",(key) => "AAAAAA" );

                await context.Response.WriteAsync("Hello, World!");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
