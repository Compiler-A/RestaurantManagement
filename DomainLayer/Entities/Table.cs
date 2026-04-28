

namespace DomainLayer.Entities
{
    public class Table
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public int StatusTableID { get; set; }
        public StatusTable? StatusTable { get; set; }
    }
}
