using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace HistoricalEventsCase.Models
{
    public class HistoricalEvent
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
    // TODO: Validations
    // 🤔: there are 3 model class where to put validation attributes?
    // tr and it endswith historicalevents model classes are takes the data from json
    // they maps the data the fetch from json source to historicalevents class by using automapper.
    // 

    /// <summary>
    /// gets the historical events data collection/table from turkish version json file
    /// </summary>
    public class HistoricalEventTr
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string dc_Zaman { get; set; }
        [Required]
        [StringLength(100)]
        public string dc_Kategori { get; set; }
        [Required]
        [StringLength(800)]
        public string dc_Olay { get; set; }
    }

    /// <summary>
    /// gets the historical events data collection/table from italian version json file
    /// </summary>
    public class HistoricalEventIt
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public string dc_Orario { get; set; }
        [Required]
        public string dc_Categoria { get; set; }
        [Required]
        public string dc_Evento { get; set; }
    }

}
