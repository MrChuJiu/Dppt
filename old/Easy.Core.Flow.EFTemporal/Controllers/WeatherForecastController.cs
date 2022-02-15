using Microsoft.AspNetCore.Mvc;

namespace Easy.Core.Flow.EFTemporal.Controllers
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

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //using (var context = new ExampleContext())
            //{
            //    #region InsertData
            //    context.AddRange(
            //        new Employee
            //        {
            //            Name = "Pinky Pie",
            //            Address = "Sugarcube Corner, Ponyville, Equestria",
            //            Department = "DevDiv",
            //            Position = "Party Organizer",
            //            AnnualSalary = 100.0m
            //        },
            //        new Employee
            //        {
            //            Name = "Rainbow Dash",
            //            Address = "Cloudominium, Ponyville, Equestria",
            //            Department = "DevDiv",
            //            Position = "Ponyville weather patrol",
            //            AnnualSalary = 900.0m
            //        },
            //        new Employee
            //        {
            //            Name = "Fluttershy",
            //            Address = "Everfree Forest, Equestria",
            //            Department = "DevDiv",
            //            Position = "Animal caretaker",
            //            AnnualSalary = 30.0m
            //        });

            //    context.SaveChanges();
            //    #endregion
            //}

            //using (var context = new ExampleContext())
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("Starting data:");

            //    var employees = context.Employees.ToList();
            //    foreach (var employee in employees)
            //    {
            //        var employeeEntry = context.Entry(employee);
            //        var validFrom = employeeEntry.Property<DateTime>("ValidFrom").CurrentValue;
            //        var validTo = employeeEntry.Property<DateTime>("ValidTo").CurrentValue;

            //        Console.WriteLine($"  Employee {employee.Name} valid from {validFrom} to {validTo}");
            //    }
            //}




   
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}