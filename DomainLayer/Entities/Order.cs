

namespace DomainLayer.Entities
{
    public class Order
    {
        public int ID { get; set; }
        public int TableID { get; set; }
        public int EmployerID { get; set; }
        public int StatusOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }


        public Table? tables { get; set; }
        public Employee? employees { get; set; }
        public StatusOrder? statusOrders { get; set; }

    }



}
