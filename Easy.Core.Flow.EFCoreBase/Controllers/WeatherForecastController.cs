using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy.Core.Flow.EFCoreBase.BaseContext;
using Easy.Core.Flow.EFCoreBase.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Easy.Core.Flow.EFCoreBase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            using (var context = new ApplicationDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            using (var db = new ApplicationDbContext()) {

               
                var blog = new SpecialBlog();
                blog.SpecialUrl = "Url";
                blog.Url = "11111";

                db.Blogs.Add(blog);
                db.SaveChanges();

            }
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
}
