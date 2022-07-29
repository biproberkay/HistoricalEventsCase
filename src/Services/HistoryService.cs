using AutoMapper;
using HistoricalEventsCase.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace HistoricalEventsCase.Services
{
    public interface IHistoryService
    {
        public IQueryable<HistoricalEvent> Almanac { get; }
        public IEnumerable<HistoricalEvent> GetHistoricalEvents(CultureInfo culture);
        // TODO: getcategories

    }
    public class HistoryService : IHistoryService
    {
        private readonly IMemoryCache _cache;
        private readonly JsonSerializer _serializer = new JsonSerializer();
        private readonly ILogger<HistoryService> _logger;
        public HistoryService(IMemoryCache cache, ILogger<HistoryService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public IQueryable<HistoricalEvent> Almanac => GetHistoricalEvents(Thread.CurrentThread.CurrentCulture).AsQueryable();
                
        public IEnumerable<HistoricalEvent> GetHistoricalEvents(CultureInfo culture)
        {
            var trCulture = CultureInfo.GetCultureInfo("tr-TR");
            var itCulture = CultureInfo.GetCultureInfo("it-IT");
            string cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_data";
            if (!_cache.TryGetValue(cacheKey,out List<HistoricalEvent> historicalEvents))
            {
                if (culture == trCulture)
                {
                    historicalEvents = GetHistoricalEventsFromTR();
                }
                else if (culture == itCulture)
                {
                    historicalEvents = GetHistoricalEventsFromIT();
                }
                else
                {
                    historicalEvents = GetHistoricalEventsFromTR();
                }
                _cache.Set(cacheKey, historicalEvents);
                _logger.LogInformation($"{cacheKey} has been cached");
            }

            return historicalEvents;
        }

        private List<HistoricalEvent> GetHistoricalEventsFromIT()
        {
            List<HistoricalEventIt> listIT = new();
            using (StreamReader sr = new StreamReader("Files\\HistoricalEvent.it-IT.json"))
            {
                string json = sr.ReadToEnd();
                listIT = JsonConvert.DeserializeObject<List<HistoricalEventIt>>(json);
            }
            var configuration = new MapperConfiguration(c =>
            {
                c.ReplaceMemberName("ID", "Id");
                c.ReplaceMemberName("dc_Orario", "Date");
                c.ReplaceMemberName("dc_Categoria", "Category");
                c.ReplaceMemberName("dc_Evento", "Description");
                c.CreateMap<HistoricalEventIt, HistoricalEvent>().ReverseMap();
            });
            configuration.AssertConfigurationIsValid();
            var _mapper = configuration.CreateMapper();
            return _mapper.Map<List<HistoricalEvent>>(listIT);
        }

        private List<HistoricalEvent> GetHistoricalEventsFromTR()
        {
            List<HistoricalEventTr> listTR = null;
            using (StreamReader sr = new StreamReader("Files\\HistoricalEvent.tr-TR.json"))
            {
                string json = sr.ReadToEnd();
                listTR = JsonConvert.DeserializeObject<List<HistoricalEventTr>>(json);
            }

            var configuration = new MapperConfiguration(c =>
            {
                c.ReplaceMemberName("ID", "Id");
                c.ReplaceMemberName("dc_Zaman", "Date");
                c.ReplaceMemberName("dc_Kategori", "Category");
                c.ReplaceMemberName("dc_Olay", "Description");
                c.CreateMap<HistoricalEventTr, HistoricalEvent>().ReverseMap();
            });
            configuration.AssertConfigurationIsValid();
            var _mapper = configuration.CreateMapper();
            
            return _mapper.Map<List<HistoricalEvent>>(listTR);
        }

       
    }
}
