using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dppt.Authorization.Samples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController: ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 获取天气
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(TestPermissions.Company.Create)]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
    public class WeatherForecast
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 摄氏温度
        /// </summary>
        public int TemperatureC { get; set; }
        /// <summary>
        /// 华氏温度
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
    }
}
