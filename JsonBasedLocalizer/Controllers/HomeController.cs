using JsonBasedLocalizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Linq;

namespace JsonBasedLocalizer.Controllers
{
    /// <summary>
    /// HomeControler
    /// </summary>
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
        /// <summary>
        /// 
        /// Supported cultures are: tr-TR, it-IT 
        /// </summary>
        /// <returns> historical event json data table key names according to selected culture </returns>
        [HttpGet(Name = "HistoricalEvents")]
        public IActionResult Get()
        {
            string message = "keys:";
            foreach (var property in typeof(HistoricalEvent).GetProperties())
            {
                message += "\n"+property.Name+" : "+ _localizer[_localizer[property.Name]].ToString();
            }
            message += "\n"+
                @$" 
                /// Current culture: {@System.Globalization.CultureInfo.CurrentCulture.DisplayName}
                /// Current UI culture: {@System.Globalization.CultureInfo.CurrentUICulture.DisplayName}
                /// Try to navigate to /{{culture}}/
                /// ";
            _logger.LogInformation(message);
            return Ok(message);
        }
    }
}