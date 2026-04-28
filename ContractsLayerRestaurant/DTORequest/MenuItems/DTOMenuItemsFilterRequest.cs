using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.MenuItems
{
    public class DTOMenuItemsFilterRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int Page { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "StatusMenuID must be greater than 0.")]
        public int StatusMenuID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "TypeItemID must be greater than 0.")]
        public int TypeItemID { get; set; }


    }
}
