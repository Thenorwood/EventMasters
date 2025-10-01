using System.ComponentModel.DataAnnotations;

namespace EventMasters.Models
{
    public class Concert
    {
    //primary key
        [Display(Name = "Event Id")]
    
        public int ConcertId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime EventDate { get; set; }

        public string Category { get; set; } = string.Empty;

        //nav property
        public List<Category>? Categories { get; set; }



    }
}
