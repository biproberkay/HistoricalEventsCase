using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JsonBasedLocalizer.Controllers
{
    [ApiController]
    [Route("{culture:culture}[controller]")]
    public class DemoController : ControllerBase
    {

        private readonly ILogger<DemoController> _logger;
        private readonly IStringLocalizer<DemoController> _localizer;
        public DemoController(ILogger<DemoController> logger, IStringLocalizer<DemoController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation(_localizer["Id"]);
            var message = _localizer["Id"].ToString();
            return Ok(message);
        }
        [HttpGet("{id}")]
        public IActionResult Get(string name)
        {
            var message = string.Format(_localizer["Description"], name);
            return Ok(message);
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var message = _localizer.GetAllStrings();
            return Ok(message);
        }
    }
}
