using System.ComponentModel.DataAnnotations;

namespace ContractsLayerRestaurant.DTOs.MenuItems
{
    public class DTOMenuItemsCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TypeItemID must be greater than 0.")]
        public int TypeItemID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusMenuID must be greater than 0.")]
        public int StatusMenuID { get; set; }
        public string? Image { get; set; }

        public DTOMenuItemsCRequest(string Name, string? Description, decimal Price, int TypeItemID, int StatusMenuID, string? Image)
        {
            this.Name = Name;
            this.Description = Description;
            this.Price = Price;
            this.TypeItemID = TypeItemID;
            this.StatusMenuID = StatusMenuID;
            this.Image = Image;
        }
    }
}
