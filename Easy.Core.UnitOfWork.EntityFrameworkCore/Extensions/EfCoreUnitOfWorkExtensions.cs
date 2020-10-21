using Easy.Core.Flow.AspectCore;
using Easy.Core.Flow.UnitOfWork.Uow;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions
{
    /// <summary>
    /// EFCore工作单元扩展函数
    /// </summary>
    public static class EfCoreUnitOfWorkExtensions
    {
        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <typeparam name="TDbContext">DbContext类型</typeparam>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>DbContext</returns>
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork)
          where TDbContext : DbContext
        {
            return unitOfWork.GetDbContext() as TDbContext;
        }

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <param name="unitOfWork">工作单元对象</param>
        /// <param name="dbContextType">DbContext类型</param>
        /// <param name="name"></param>
        /// <returns>DbContext</returns>
        public static DbContext GetDbContext(this IActiveUnitOfWork unitOfWork)
        {
            Check.NotNull(unitOfWork, nameof(unitOfWork));

            if (!(unitOfWork is EfCoreUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfCoreUnitOfWork).FullName, "unitOfWork");
            }


            return (unitOfWork as EfCoreUnitOfWork).GetOrCreateDbContext();
        }

        /// <summary>
        /// 切换 DbContextProviderName
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="name">provider name</param>
        /// <returns></returns>
        public static IDisposable SetDbContextProvider(this IActiveUnitOfWork unitOfWork, string name)
        {
            Check.NotNull(unitOfWork, nameof(unitOfWork));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return (unitOfWork as EfCoreUnitOfWork).SetDbContextProvider(name);
        }

    }
}
