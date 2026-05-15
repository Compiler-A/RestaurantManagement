
namespace DomainLayer.Entities
{
    public class TypeItem
    {
        public int TypeItemID { get; set; }
        public string TypeName { get; set; } = null!;
        public string? TypeDescription { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    }

}
