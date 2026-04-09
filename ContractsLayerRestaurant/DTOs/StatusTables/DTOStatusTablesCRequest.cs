using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.StatusTables
{
    public class DTOStatusTablesCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public DTOStatusTablesCRequest(string Name)
        {
            this.Name = Name;
        }
    }
}
