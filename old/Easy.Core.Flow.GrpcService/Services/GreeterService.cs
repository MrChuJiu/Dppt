using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Easy.Core.Flow.GrpcService
{
    /// <summary>
    /// ����.proto�������ķ���
    /// GreeterService�������ⶨ��
    /// Greeter.GreeterBase ����.proto�ļ��ж���Ĺ�����
    /// </summary>
    public class GreeterService : Greeter.GreeterBase
    {
        // ��ASP.NETCoreһ��������ʹ������ע��ͷ���
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// ��д ��ƶ�Ӧ�Ķ���ӿ�
        /// һ�㶼���첽����
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
