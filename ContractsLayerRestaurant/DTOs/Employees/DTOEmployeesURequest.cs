using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.Employees
{
    public class DTOEmployeesURequest : DTOEmployeesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0")]
        public int ID { get; set; }

        public DTOEmployeesURequest(int ID, string Name, int JobID, string userName, string password)
            : base(Name, JobID, userName, password)
        {
            this.ID = ID;
        }

    }
}
