using Easy.Core.Flow.AspectCore;
using Easy.Core.Flow.UnitOfWork.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions
{
    public static class EfCoreUnitOfWorkOptionsExtensions
    {
        /// <summary>
        /// 获取工作单元选项扩展信息中的 DbContext Provider Name
        /// </summary>
        /// <param name="unitOfWorkOptions"></param>
        /// <returns></returns>
        public static string GetDbContextProviderName(this UnitOfWorkOptions unitOfWorkOptions)
        {
            if (unitOfWorkOptions.ExtraData.TryGetValue(RivenUnitOfWorkEntityFrameworkCoreConsts.UnitOfWorkOptionsExtraDataDbContextProviderName, out object result))
            {
                return result.ToString();
            }


            return RivenUnitOfWorkEntityFrameworkCoreConsts.DefaultDbContextProviderName;
        }

        /// <summary>
        /// 设置工作单元选项扩展信息中的 DbContext Provider Name
        /// </summary>
        /// <param name="unitOfWorkOptions"></param>
        /// <param name="name"></param>
        public static void SetDbContextProviderName(this UnitOfWorkOptions unitOfWorkOptions, string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            unitOfWorkOptions.ExtraData[RivenUnitOfWorkEntityFrameworkCoreConsts.UnitOfWorkOptionsExtraDataDbContextProviderName] = name;
        }

    }
}
