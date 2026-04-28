

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOMenuItemResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int TypeItemID { get; set; }
        public int StatusMenuID { get; set; }

        public string? Image { get; set; }
        public DTOTypeItemResponse? TypeItems { get; set; }
        public DTOStatusMenuResponse? StatusMenus { get; set; }
    }
}
