using ContractsLayerRestaurant.DTORequest.People;
using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Employees
{
    public class DTOEmployeesCRequest : DTOPeopleRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "JobID must be greater than 0")]
        public int JobID { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
