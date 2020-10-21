using Easy.Core.Flow.AspNetCore.Mvc.Uow;
using Easy.Core.Flow.UnitOfWork.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.Uow
{
    public static class RivenAspNetCoreUowExtensions
    {
        public static IServiceCollection AddRivenAspNetCoreUow(this IServiceCollection services, Action<UnitOfWorkAttribute> optionsAction = null) {

            // 获取一个选项生成器，以便将同一命名 TOptions 的配置调用转发到基础服务集合。
            services.AddOptions<UnitOfWorkAttribute>();
            if (optionsAction != null)
            {
                // 注册用于配置特定类型的选项的操作。 这些都在
                // PostConfigure<TOptions>(IServiceCollection, Action<TOptions>) 之前运行。
                services.Configure(optionsAction);
            }
            // 使用瞬时模式进行注册
            services.TryAddTransient<AspNetCoreUowMiddleware>();

            return services;
        }
    }
}
