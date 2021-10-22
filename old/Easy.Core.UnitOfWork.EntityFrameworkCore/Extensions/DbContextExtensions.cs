using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;
using System.Linq;
using Easy.Core.Flow.AspectCore.Reflection;
using Easy.Core.Domain.Entities;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions
{
    public static class DbContextExtensions
    {
        public static bool HasRelationalTransactionManager(this DbContext dbContext)
        {
            return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
        }

        public static IEnumerable<EntityTypeInfo> GetEntityTypeInfos(this Type dbContextType)
        {
            return
                from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    (ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) ||
                     ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbQuery<>))) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                        typeof(IEntity<>))
                select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }
    }
}
