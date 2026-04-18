using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.Auth
{
    public class DTOLoginCURequest
    {
        public int EmployeeID { get; set; }
        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
