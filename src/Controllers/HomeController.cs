using AutoMapper;
using HistoricalEventsCase.Infrastructure.BasicAuth;
using HistoricalEventsCase.Models;
using HistoricalEventsCase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace HistoricalEventsCase.Controllers
{
    /// <summary>
    /// HomeControler
    /// </summary>
    [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
    [ApiController]
    [Route("{culture:culture}/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHistoryService _historyService;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger,
            IHistoryService historyService,
            IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _historyService = historyService;
            _localizer = localizer;
        }
#if false // DI from HistoryService
// inject when without HistoryService
        // private readonly IMemoryCache _cache;
            // IMemoryCache cache,
            // _cache = cache;
        private List<HistoricalEvent> GetHistoricalEvents(CultureInfo culture)
        {
            var trCulture = CultureInfo.GetCultureInfo("tr-TR");
            var itCulture = CultureInfo.GetCultureInfo("it-IT");
            List<HistoricalEvent> historicalEvents = null;
            string cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_data";
            if (!_cache.TryGetValue(cacheKey, out historicalEvents))
            {
                if (culture == trCulture)
                {
                    var configuration = new MapperConfiguration(c =>
                    {
                        c.ReplaceMemberName("ID", "Id");
                        c.ReplaceMemberName("dc_Zaman", "Date");
                        c.ReplaceMemberName("dc_Kategori", "Category");
                        c.ReplaceMemberName("dc_Olay", "Description");
                        c.CreateMap<HistoricalEventTr, HistoricalEvent>().ReverseMap();
                    });
                    var _mapper = configuration.CreateMapper();
                    historicalEvents = _mapper.Map<List<HistoricalEvent>>(GetHistoricalEventsTR());
                }
                else if (culture == itCulture)
                {
                    var configuration = new MapperConfiguration(c =>
                    {
                        c.ReplaceMemberName("ID", "Id");
                        c.ReplaceMemberName("dc_Orario", "Date");
                        c.ReplaceMemberName("dc_Categoria", "Category");
                        c.ReplaceMemberName("dc_Evento", "Description");
                        c.CreateMap<HistoricalEventIt, HistoricalEvent>().ReverseMap();
                    });
                    var _mapper = configuration.CreateMapper();
                    historicalEvents = _mapper.Map<List<HistoricalEvent>>(GetHistoricalEventsIT());
                }
                else
                {
                    var configuration = new MapperConfiguration(c =>
                    {
                        c.ReplaceMemberName("ID", "Id");
                        c.ReplaceMemberName("dc_Zaman", "Date");
                        c.ReplaceMemberName("dc_Kategori", "Category");
                        c.ReplaceMemberName("dc_Olay", "Description");
                        c.CreateMap<HistoricalEventTr, HistoricalEvent>().ReverseMap();
                    });
                    var _mapper = configuration.CreateMapper();
                    historicalEvents = _mapper.Map<List<HistoricalEvent>>(GetHistoricalEventsTR());
                }
                _cache.Set(cacheKey, historicalEvents);
            }

            return historicalEvents;
        }

        private object GetHistoricalEventsIT()
        {
            List<HistoricalEventIt> json = new();
            using (WebClient wc = new WebClient())
            {
                var jsonStream = wc.OpenRead("https://s3.us-west-2.amazonaws.com/secure.notion-static.com/8febcaa6-c2f8-4fab-b05b-141bafe4d344/1d6a2360.json?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20220723%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20220723T074930Z&X-Amz-Expires=86400&X-Amz-Signature=31082d42eb2f56e14019a9bfecd0b1db5c03410884f2f08367d7ea564f8d80c8&X-Amz-SignedHeaders=host&response-content-disposition=filename%20%3D%221d6a2360.json%22&x-id=GetObject");
                var jsonReader = new StreamReader(jsonStream);
                var textReader = new JsonTextReader(jsonReader);
                textReader.Read();
                json = _serializer.Deserialize<List<HistoricalEventIt>>(textReader);
            }
            return json;
        }

        private List<HistoricalEventTr> GetHistoricalEventsTR()
        {
            List<HistoricalEventTr> json = null;
            using (WebClient wc = new WebClient())
            {
                var jsonStream = wc.OpenRead("https://s3.us-west-2.amazonaws.com/secure.notion-static.com/c86e0795-cfbb-42b9-8164-739f72ebf585/3455dde5.json?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20220723%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20220723T223135Z&X-Amz-Expires=86400&X-Amz-Signature=9c3a32c8aa99492a9d49823e3d1ed66b2cc497797d168d05ede394eb96732ec3&X-Amz-SignedHeaders=host&response-content-disposition=filename%20%3D%223455dde5.json%22&x-id=GetObject");
                var jsonReader = new StreamReader(jsonStream);
                var textReader = new JsonTextReader(jsonReader);
                textReader.Read();
                json = _serializer.Deserialize<List<HistoricalEventTr>>(textReader);
            }
            return json;
        }

        private readonly JsonSerializer _serializer = new JsonSerializer();
        private List<HistoricalEvent> _historicalEvents => GetHistoricalEvents(CultureInfo.CurrentCulture);
#endif
        /// <summary>
        /// 
        /// Supported cultures are: tr-TR, it-IT 
        /// </summary>
        /// <returns> historical event json data table key names according to selected culture </returns>
        [HttpGet(Name = "HistoricalEvents")]
        public IEnumerable<HistoricalEvent> Get()
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
            List<HistoricalEvent> result = _historyService.GetHistoricalEvents(Thread.CurrentThread.CurrentCulture).ToList();
            _logger.LogInformation($"we are getting 3-15 event from {result.Count} events");
            return result.OrderBy(x => Random.Shared.Next(result.Count)).Take(Random.Shared.Next(3, 15));
        }
    }
}