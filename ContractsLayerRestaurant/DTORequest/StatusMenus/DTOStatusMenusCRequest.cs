using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.StatusMenus
{
    public class DTOStatusMenusCRequest
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

}
