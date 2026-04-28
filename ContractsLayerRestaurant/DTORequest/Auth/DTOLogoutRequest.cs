namespace ContractsLayerRestaurant.DTORequest.Auth
{
    public class DTOLogoutRequest
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}