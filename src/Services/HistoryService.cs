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
        public IEnumerable<HistoricalEvent> GetHistoricalEvents(CultureInfo culture);
    }
    public class HistoryService : IHistoryService
    {
        private readonly IMemoryCache _cache;
        private readonly JsonSerializer _serializer = new JsonSerializer();
        public HistoryService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public IEnumerable<HistoricalEvent> GetHistoricalEvents(CultureInfo culture)
        {
            var trCulture = CultureInfo.GetCultureInfo("tr-TR");
            var itCulture = CultureInfo.GetCultureInfo("it-IT");
            List<HistoricalEvent> historicalEvents = null;
            string cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_data";
            if(!_cache.TryGetValue(cacheKey,out historicalEvents))
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

    }
}
