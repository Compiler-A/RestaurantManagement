

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOMenuItemResponse
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string StatusMenuName { get; set; } = string.Empty;
    }
}
