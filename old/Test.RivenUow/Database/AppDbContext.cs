using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.RivenUow.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext() { 
        
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {

        }
        public virtual IServiceProvider ServiceProvider { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="serviceProvider"></param>
        public AppDbContext(DbContextOptions options, IServiceProvider serviceProvider = null)
            : base(options)
        {
            ServiceProvider = serviceProvider;
        }
        public DbSet<User> Users { get; set; }
    }
}
