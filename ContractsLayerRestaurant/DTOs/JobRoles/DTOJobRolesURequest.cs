using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.JobRoles
{
    public class DTOJobRolesURequest : DTOJobRolesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }

        public DTOJobRolesURequest(int ID, string Name, string Description) : base(Name, Description)
        {
            this.ID = ID;
        }
    }
}
