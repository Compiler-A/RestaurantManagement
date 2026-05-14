

namespace DomainLayer.Entities
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public Order Order { get; set; } = null!;
        public MenuItem Item { get; set; } = null!;
    }
}
