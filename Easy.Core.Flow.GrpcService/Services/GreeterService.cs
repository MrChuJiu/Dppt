using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Easy.Core.Flow.GrpcService
{
    /// <summary>
    /// 根据.proto定义具体的服务
    /// GreeterService可以任意定义
    /// Greeter.GreeterBase 根据.proto文件中定义的规则来
    /// </summary>
    public class GreeterService : Greeter.GreeterBase
    {
        // 和ASP.NETCore一样，可以使用依赖注入和服务
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 重写 设计对应的多个接口
        /// 一般都是异步处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
