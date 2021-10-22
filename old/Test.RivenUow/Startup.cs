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
                   

                    using (var outerUOW1 = unitOfWorkManager.Begin())  // 这里返回的是 IOC 解析出的 IUnitOfWork
                    {
                        var appContext1 = unitOfWorkManager.Current.GetDbContext<AppDbContext>();

                        var user1 = new User();
                        user1.Creator = "Creator1111";
                        appContext1.Users.Add(user1);
                        using (var innerUOW2 = unitOfWorkManager.Begin())  // 内部 UOW
                        {
                            var appContext2 = unitOfWorkManager.Current.GetDbContext<AppDbContext>();
                            var user2 = new User();
                            user2.Creator = "Creator222";
                            appContext2.Users.Add(user2);

                            using (var innerUOW3 = unitOfWorkManager.Begin())  // 内部 UOW
                            {
                                var appContext3 = unitOfWorkManager.Current.GetDbContext<AppDbContext>();

                                var user3 = new User();
                                user3.Creator = "Creator333";
                                appContext3.Users.Add(user3);
                                await innerUOW3.CompleteAsync();
                            }
                            await innerUOW2.CompleteAsync();
                        }
                        await outerUOW1.CompleteAsync();
                    }
                }

            });



        }
    }
}
