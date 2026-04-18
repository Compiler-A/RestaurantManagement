namespace ContractsLayerRestaurant.DTOs.Auth
{
    public class DTORefreshRequest
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}