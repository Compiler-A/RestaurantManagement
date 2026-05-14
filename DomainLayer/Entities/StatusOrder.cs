
namespace DomainLayer.Entities
{
    public class StatusOrder
    {
        public int StatusOrderID { get; set; }
        public string StatusOrderName { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
