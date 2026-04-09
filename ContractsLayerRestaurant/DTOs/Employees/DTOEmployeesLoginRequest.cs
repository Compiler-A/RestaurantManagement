using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.Employees
{
    public class DTOEmployeesLoginRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public DTOEmployeesLoginRequest(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
    }
}
