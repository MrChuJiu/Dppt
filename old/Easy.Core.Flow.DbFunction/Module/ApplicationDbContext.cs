using Easy.Core.Flow.DbFunction.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Easy.Core.Flow.DbFunction
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Blog> Blog { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<Comment> Comment { get; set; }



        public int ActivePostCountForBlog(int blogId) => throw new NotSupportedException();

        public double PercentageDifference(double first, int second) => throw new NotSupportedException();

        public IQueryable<Post> PostsWithPopularComments(int likeThreshold) => FromExpression(() => PostsWithPopularComments(likeThreshold));


        public string DataDecrypt(string data) => throw new NotSupportedException();

        public string DataEncrypt(string data) => throw new NotSupportedException();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts)
                .WithOne(p => p.Blog);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post);
 
            modelBuilder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(PostsWithPopularComments), new[] { typeof(int) }));

            modelBuilder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(ActivePostCountForBlog), new[] { typeof(int) })).HasName("CommentedPostCountForBlog");

            modelBuilder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(PercentageDifference), new[] { typeof(double), typeof(int) }))
                            .HasTranslation(
                                args =>
                                    new SqlBinaryExpression(
                                        ExpressionType.Multiply,
                                        new SqlConstantExpression(
                                            Expression.Constant(100),
                                            new IntTypeMapping("int", DbType.Int32)),
                                        new SqlBinaryExpression(
                                            ExpressionType.Divide,
                                            new SqlFunctionExpression(
                                                "ABS",
                                                new SqlExpression[]
                                                {
                                                    new SqlBinaryExpression(
                                                        ExpressionType.Subtract,
                                                        args.First(),
                                                        args.Skip(1).First(),
                                                        args.First().Type,
                                                        args.First().TypeMapping)
                                                },
                                                nullable: true,
                                                argumentsPropagateNullability: new[] { true, true },
                                                type: args.First().Type,
                                                typeMapping: args.First().TypeMapping),
                                            new SqlBinaryExpression(
                                                ExpressionType.Divide,
                                                new SqlBinaryExpression(
                                                    ExpressionType.Add,
                                                    args.First(),
                                                    args.Skip(1).First(),
                                                    args.First().Type,
                                                    args.First().TypeMapping),
                                                new SqlConstantExpression(
                                                    Expression.Constant(2),
                                                    new IntTypeMapping("int", DbType.Int32)),
                                                args.First().Type,
                                                args.First().TypeMapping),
                                            args.First().Type,
                                            args.First().TypeMapping),
                                        args.First().Type,
                                        args.First().TypeMapping));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFCoreDbFunction.Disconnected;Trusted_Connection=True;ConnectRetryCount=0");
        }
    }
}
