using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.StatusMenus
{
    public class DTOStatusMenusCRequest
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOStatusMenusCRequest(string Name, string? Description)
        {
            this.Name = Name;
            this.Description = Description;
        }
    }

}
