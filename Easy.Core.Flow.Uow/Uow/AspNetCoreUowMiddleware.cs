using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using Microsoft.Extensions.Options;
using Easy.Core.Flow.UnitOfWork.Uow;

namespace Easy.Core.Flow.AspNetCore.Mvc.Uow
{
    public class AspNetCoreUowMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // 拿到路由数据
            if (context.Request.RouteValues.Count == 0)
            {
                await next(context);
                return;
            }
            var serviceProvider = context.RequestServices;
            /*
             自定义中间件提前拿到终结点数据
             通过httpContext.GetEndpoint()验证了是否已经匹配到了正确的Endpoint并交个下个中间件继续执行
             */
            var endpoint = context.GetEndpoint();
            /*
             * https://blog.csdn.net/sD7O95O/article/details/103776069?utm_medium=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.channel_param&depth_1-utm_source=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.channel_param
             终结点路由的理解
             路由的根本目的是将用户请求地址，映射为一个请求处理器，
             最简单的请求处理器可以是一个委托 Func<HttpCotnext,Task>，
             也可以是mvc/webapi中某个controller的某个action，
             所以从抽象的角度讲 一个终结点 就是一个处理请求的委托。
             由于mvc中action上还有很多attribute，因此我们的终结点还应该提供一个集合，用来存储与此请求处理委托的关联数据

             从抽象的角度可以简单理解为   
             一个终结点 = 处理请求的委托 + 与之关联的附加（元）数据。
             对应到mvc来理解的话 终结点 = action + 应用其上的attribute集合。
             */
            var unitOfWorkAttribute = endpoint?.Metadata?.GetMetadata<UnitOfWorkAttribute>();
            // 为空的话获取默认的工作单元配置
            if (unitOfWorkAttribute == null)
            {
                unitOfWorkAttribute = serviceProvider.GetRequiredService<IOptions<UnitOfWorkAttribute>>().Value;
            }

            // 如果是禁用
            if (unitOfWorkAttribute.IsDisabled)
            {
                await next(context);
                return;
            }

            // 当前连接字符串名称
            var currentConnectionStringName = serviceProvider.GetService<ICurrentConnectionStringNameProvider>()?.Current;


            // 创建选项
            var unitOfWorkOptions = unitOfWorkAttribute.CreateOptions(currentConnectionStringName);
            // 启动工作单元
            var unitOfWorkManager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();
            using (var uow = unitOfWorkManager.Begin(unitOfWorkOptions))
            {
                await next(context);

                // 只有响应码为200的时候才提交更改
                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    await uow.CompleteAsync(context.RequestAborted);
                }
            }


        }
    }
}
