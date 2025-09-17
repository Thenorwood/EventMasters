namespace EventMasters.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public int Capacity { get; set; }   

    }
}
