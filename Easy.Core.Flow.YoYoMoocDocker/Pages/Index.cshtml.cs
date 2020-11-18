using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy.Core.Flow.GrpcService;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Easy.Core.Flow.YoYoMoocDocker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            //// 创建通道
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //// 发起客户端调用
            //var client = new Greeter.GreeterClient(channel);
            //// api请求，传递参数
            //var response = await client.SayHelloAsync(new HelloRequest { Name = "World" });

            //Console.WriteLine("Greeting: " + response.Message);
        }
    }
}
