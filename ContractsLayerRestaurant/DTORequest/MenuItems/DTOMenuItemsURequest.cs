using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.MenuItems
{
    public class DTOMenuItemsURequest : DTOMenuItemsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }

}
