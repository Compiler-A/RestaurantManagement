using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.JobRoles
{
    public class DTOJobRoles
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOJobRoles(int ID, string name, string? description)
        {
            this.ID = ID;
            this.Name = name;
            this.Description = description;
        }
        public DTOJobRoles()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
        }
    }
}
