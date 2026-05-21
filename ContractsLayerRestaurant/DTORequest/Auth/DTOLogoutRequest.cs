namespace ContractsLayerRestaurant.DTORequest.Auth
{
    public class DTOLogoutRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}