using Easy.Core.Flow.EFCoreBase.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Core.Flow.EFCoreBase.BaseContext
{
    public class ApplicationDbContext: DbContext
    {
      //  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      //: base(options)
      //  {
      //  }


        public DbSet<Blog> Blogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasDiscriminator()
                .HasValue<ResBlog>(nameof(ResBlog))
                .HasValue<SpecialBlog>(nameof(SpecialBlog));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFCoreBase_Db.Disconnected;Trusted_Connection=True;ConnectRetryCount=0");
        }


    }
}
