namespace ContractsLayerRestaurant.DTORequest.Auth
{
    public class DTORefreshRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}