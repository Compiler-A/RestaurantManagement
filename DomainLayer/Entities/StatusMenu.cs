

namespace DomainLayer.Entities
{
    public class StatusMenu
    {
        public int StatusMenuID { get; set; }
        public string StatusMenuName { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }

}
