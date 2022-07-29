using HistoricalEventsCase.Infrastructure.BasicAuth;
using HistoricalEventsCase.Models;
using HistoricalEventsCase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HistoricalEventsCase.Controllers
{
    /// <summary>
    /// HomeControler
    /// </summary>
    [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
    [ApiController]
    [Route("{culture:culture}/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly ILogger<HistoryController> _logger;
        private readonly IHistoryService _historyService;
        private readonly IStringLocalizer<HistoryController> _localizer;

        public HistoryController(ILogger<HistoryController> logger,
            IHistoryService historyService,
            IStringLocalizer<HistoryController> localizer)
        {
            _logger = logger;
            _historyService = historyService;
            _localizer = localizer;
        }

        /// <summary>
        /// 
        /// Supported cultures are: tr-TR, it-IT 
        /// </summary>
        /// <returns> historical event json data table key names according to selected culture </returns>
        [HttpGet(Name = "random")]
        public IEnumerable<HistoricalEvent> Get()
        {
            string message = "keys:";
            foreach (var property in typeof(HistoricalEvent).GetProperties())
            {
                message += "\n" + property.Name + " : " + _localizer[_localizer[property.Name]].ToString();
            }
            message += "\n" +
                @$" 
                /// Current culture: {@System.Globalization.CultureInfo.CurrentCulture.DisplayName}
                /// Current UI culture: {@System.Globalization.CultureInfo.CurrentUICulture.DisplayName}
                /// Try to navigate to /{{culture}}/
                /// 🗒Categories:
                /// {_historyService.Almanac.Select(x => x.Category).Distinct().ToArray()}";
            _logger.LogInformation(message);

            List<HistoricalEvent> result = _historyService.Almanac.ToList();

            _logger.LogInformation($"we are getting 3-15 event from {result.Count} events");
            return result.OrderBy(x => Random.Shared.Next(result.Count)).Take(Random.Shared.Next(3, 15));
        }

        // DONE: ONDONE: TODO: getallcategories endpoint 🤨 maybe there is no need to do this.
        // i will use logger 
        [HttpGet("categories")]
        public string[] GetCategories()
        {
            return _historyService.Almanac.Select(x => x.Category).Distinct().ToArray();
        }
        // UNDONE: getbyId
        // UNDONE: getbyDate

        // DONE: UNDONE: search
        /// <summary>
        /// searches query string in values
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public IActionResult Search(string queryString)
        {
            queryString = queryString.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(queryString))
                return BadRequest();

            var query = _historyService.Almanac.Where(e =>
                e.Date.ToLower().Contains(queryString) ||
                e.Category.ToLower().Contains(queryString) ||
                e.Description.ToLower().Contains(queryString));
            if (!query.Any())
                return Ok(_localizer["NoSearchResult"]);
            return Ok(query);
        }
        // 🤔: date key has no year value



    }
}