using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.Employees
{
    public class DTOEmployeesCRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "JobID must be greater than 0")]
        public int JobID { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public DTOEmployeesCRequest(string name, int jobID, string userName, string password)
        {
            Name = name;
            JobID = jobID;
            UserName = userName;
            Password = password;
        }
    }
}
