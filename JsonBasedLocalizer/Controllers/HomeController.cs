using JsonBasedLocalizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Linq;

namespace JsonBasedLocalizer.Controllers
{
    [ApiController]
    [Route("{culture:culture}/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _cache;
        private readonly JsonSerializer _serializer = new JsonSerializer();
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _cache = cache;
            _localizer = localizer;
        }

        [HttpGet(Name = "HistoricalEvents")]
        public IActionResult Get()
        {
            string message = "keys:";
            foreach (var property in typeof(HistoricalEvent).GetProperties())
            {
                _logger.LogInformation(_localizer[property.Name]);
                message += "\n"+property.Name+" : "+ _localizer[_localizer[property.Name]].ToString();
            }
            return Ok(message);
        }
    }
}