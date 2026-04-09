
namespace ContractsLayerRestaurant.DTOs.TypeItems
{
    public class DTOTypeItems
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOTypeItems()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
        }

        public DTOTypeItems(int typeItemID, string typeItemName, string? typeItemDescription)
        {
            ID = typeItemID;
            Name = typeItemName;
            Description = typeItemDescription;
        }
    }

}
