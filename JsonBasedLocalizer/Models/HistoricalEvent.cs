using Microsoft.Extensions.Localization;

namespace JsonBasedLocalizer.Models
{
    public class HistoricalEvent
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }


    public class HistoricalEventTr
    {
        public int ID { get; set; }
        public string dc_Zaman { get; set; }
        public string dc_Kategori { get; set; }
        public string dc_Olay { get; set; }
    }


    public class HistoricalEventIt
    {
        public int ID { get; set; }
        public string dc_Orario { get; set; }
        public string dc_Categoria { get; set; }
        public string dc_Evento { get; set; }
    }

}
