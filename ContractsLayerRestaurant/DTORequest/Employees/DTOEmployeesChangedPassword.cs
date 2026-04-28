using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTORequest.Employees
{
    public class DTOEmployeesChangedPassword
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0")]
        public int ID { get; set; }

        [Required(ErrorMessage = "CurrentPassword is required")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "NewPassword is required")]
        public string NewPassword { get; set; } = string.Empty;

        public DTOEmployeesChangedPassword(int ID, string CurrentPassword, string NewPassword)
        {
            this.ID = ID;
            this.CurrentPassword = CurrentPassword;
            this.NewPassword = NewPassword;
        }
    }
}
