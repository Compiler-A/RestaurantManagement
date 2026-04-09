

namespace ContractsLayerRestaurant.DTOs.StatusMenus
{
    public class DTOStatusMenus
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOStatusMenus()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
        }

        public DTOStatusMenus(int statusMenuID, string statusMenuName, string? statusMenuDescription)
        {
            ID = statusMenuID;
            Name = statusMenuName;
            Description = statusMenuDescription;
        }
    }

}
