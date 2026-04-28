
using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.TypeItems
{
    public class DTOTypeItemsCRequest
    {
        [Required(ErrorMessage = "Name is Required.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOTypeItemsCRequest(string name, string? description)
        {
            Name = name;
            Description = description;
        }
    }
}
