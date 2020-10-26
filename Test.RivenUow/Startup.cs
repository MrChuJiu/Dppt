using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Easy.Core.Flow.Uow;
using Easy.Core.UnitOfWork.EntityFrameworkCore;
using Test.RivenUow.Database;
using Easy.Core.Flow.UnitOfWork;
using Easy.Core.Flow.UnitOfWork.Uow;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions;

namespace Test.RivenUow
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

            services.AddDefaultConnectionString(Configuration["ConnectionStrings:Default"]);

            services.AddRivenAspNetCoreUow();
            services.AddUnitOfWorkWithEntityFrameworkCore();
            services.AddUnitOfWorkWithEntityFrameworkCoreRepository();
            services.AddUnitOfWorkWithEntityFrameworkCoreDefaultDbContext<AppDbContext>((config) =>
            {
                // 这个在每次需要创建DbContext的时候执行
                if (config.ExistingConnection != null)
                {
                    config.DbContextOptions.Configure(config.ExistingConnection);
                }
                else
                {
                    config.DbContextOptions.Configure(config.ConnectionString);
                }
            });
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


            app.UseRivenAspnetCoreUow();




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Task.Run(async () =>
            {


                using (var scope = app.ApplicationServices.CreateScope())
                {

                    var scopeServiceProvider = scope.ServiceProvider;

                    var unitOfWorkManager = scopeServiceProvider.GetService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {

                        var appContext = unitOfWorkManager.Current.GetDbContext<AppDbContext>();



                        await uow.CompleteAsync();
                    }
                }

            });



        }
    }
}
