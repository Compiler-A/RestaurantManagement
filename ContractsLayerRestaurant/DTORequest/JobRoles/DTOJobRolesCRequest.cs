using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.JobRoles
{
    public class DTOJobRolesCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
