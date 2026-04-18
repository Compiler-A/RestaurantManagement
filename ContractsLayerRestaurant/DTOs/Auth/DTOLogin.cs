using ContractsLayerRestaurant.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.Auth
{
    public class DTOLogin
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public DTOEmployees? Employees { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
    }
}
