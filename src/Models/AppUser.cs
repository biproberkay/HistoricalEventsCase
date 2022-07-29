using System.ComponentModel.DataAnnotations;

namespace HistoricalEventsCase.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
