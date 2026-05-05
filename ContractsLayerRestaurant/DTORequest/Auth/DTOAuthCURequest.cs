

namespace ContractsLayerRestaurant.DTORequest.Auth
{
    public class DTOAuthCURequest
    {
        public int EmployeeID { get; set; }
        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
