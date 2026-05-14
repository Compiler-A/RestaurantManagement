
namespace DomainLayer.Entities
{
    public class StatusTable
    {
        public int StatusTableID { get; set; }
        public string StatusTableName { get; set; } = null!;

        public ICollection<Table> Tables { get; set; } = new List<Table>();
    }
}
