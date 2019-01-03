using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace IoTSink.Controllers
{
    public class WeatherContract
    {
        public double Temp;
        public double Co2;
        public double Pressure;
        public double Humidity;
        public DateTime? Date;
        public int DeviceId = 0;
    }
    
    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IConfiguration _configuration;

        
        private readonly IMongoCollection<WeatherContract> _weartherCollection;
        
        public WeatherController(ILogger<WeatherController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            //TODO: Move to IOC
            var mongoClient = new MongoClient(MongoUrl.Create(_configuration["ConnectionString"]));
            var db = mongoClient.GetDatabase("iot");
            _weartherCollection = db.GetCollection<WeatherContract>("weather");
        }
        [HttpPost]
        public void Post([FromBody] WeatherContract weather)
        {
            weather.Date = DateTime.UtcNow;
            
           _logger.LogInformation($"Co2={weather.Co2} Hum={weather.Humidity} Pressure={weather.Pressure} Temp={weather.Temp}");
            
            _weartherCollection.InsertOne(weather);

        }

       
    }
}