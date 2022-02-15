using Easy.Core.Flow.DbFunction.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Core.Flow.DbFunction.Controllers
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
            var rng = new Random();

            using (var context = new ApplicationDbContext())
            {

                var query1 = (from b in context.Blog
                              where context.ActivePostCountForBlog(b.BlogId) > 1
                              select b).ToList();


                var likeThreshold = 3;
                var query2 = (from p in context.PostsWithPopularComments(likeThreshold)
                              orderby p.Rating
                              select p).ToList();


                //var query3 = context.Blog.Select(x => new
                //{

                //    Url = context.Blog.FromSqlRaw($"exec DataEncrypt  @param1='sdssss'").FirstOrDefault()

                //}).ToList();


            }

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
