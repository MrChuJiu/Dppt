using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Test.RivenUow.Database
{
    public class AppDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var connectionString = configuration["ConnectionStrings:Default"];


            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} start");
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} current database connection string: {connectionString}");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            AppDbContextConfigurer.Configure(optionsBuilder, connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
  
    }
}
