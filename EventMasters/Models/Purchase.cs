namespace EventMasters.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int ConcertId { get; set; }

        public int Quantity { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }

        public string? CardType { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Concert Concert { get; set; } = null!; //for navigation
    }
}
