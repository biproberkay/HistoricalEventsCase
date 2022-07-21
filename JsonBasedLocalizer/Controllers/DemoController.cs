using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JsonBasedLocalizer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {

        private readonly ILogger<DemoController> _logger;
        private readonly IStringLocalizer<DemoController> _loc;
        public DemoController(ILogger<DemoController> logger, IStringLocalizer<DemoController> loc)
        {
            _logger = logger;
            _loc = loc;
        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation(_loc["dc_Kategori"]);
            var message = _loc["dc_Kategori"].ToString();
            return Ok(message);
        }
        [HttpGet("{id}")]
        public IActionResult Get(string name)
        {
            var message = string.Format(_loc["dc_Olay"], name);
            return Ok(message);
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var message = _loc.GetAllStrings();
            return Ok(message);
        }
    }
}
