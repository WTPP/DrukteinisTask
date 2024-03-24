using System;

namespace Models.DTO
{
    public class PurchaseDTO
    {
        public int BasketId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double Sum { get; set; }
        public DateTime? Date { get; set; }
    }
}
