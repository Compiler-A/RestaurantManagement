

namespace DomainLayer.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public int TableID { get; set; }
        public int EmployeeID { get; set; }
        public int StatusOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public Table Table { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public StatusOrder StatusOrder { get; set; } = null!;

    }



}
