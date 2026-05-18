using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.StatusTables
{
    public class DTOStatusTablesCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}
