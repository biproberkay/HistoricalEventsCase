using Microsoft.Extensions.Localization;

namespace JsonBasedLocalizer.Models
{
    public class HistoricalEvent
    {
        private readonly IStringLocalizer<HistoricalEvent> _localizer;
        public HistoricalEvent(IStringLocalizer<HistoricalEvent> localizer)
        {
            _localizer = localizer;
        }
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

    public class UnNamed
    {
        public System.Globalization.CultureInfo CultureInfo { get; set; }
        public string Resource { get; set; }
    }
}
