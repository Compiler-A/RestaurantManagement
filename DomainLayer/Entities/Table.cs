

namespace DomainLayer.Entities
{
    public class Table
    {
        public int TableID { get; set; }
        public string TableNumber { get; set; } = null!;
        public int Seats { get; set; }
        public int StatusTableID { get; set; }
        public StatusTable StatusTable { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
