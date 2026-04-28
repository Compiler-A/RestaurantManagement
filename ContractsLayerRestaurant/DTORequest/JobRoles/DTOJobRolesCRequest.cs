using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTORequest.JobRoles
{
    public class DTOJobRolesCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOJobRolesCRequest(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
        }
    }
}
