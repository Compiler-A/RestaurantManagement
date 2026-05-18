using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.OrderDetails
{
    public class DTOOrderDetailsURequest : DTOOrderDetailsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }
}
