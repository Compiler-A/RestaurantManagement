

namespace DomainLayer.Entities
{
    public class MenuItem
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int TypeItemID { get; set; }
        public int StatusMenuID { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public string? Image { get; set; }
        public TypeItem TypeItem { get; set; } = null!;
        public StatusMenu StatusMenu { get; set; } = null!;
    }
}
