using ContractsLayerRestaurant.DTORequest.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTORequest.Auth
{
    public class Auth
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public Employees.Employee? Employees { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
    }
}
