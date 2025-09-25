using System.ComponentModel;

namespace EventMasters.Models
{
    public class Concert
    {
        public int ConcertId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int VenueId { get; set; }

        public int CategoryId { get; set; } 



    }
}
