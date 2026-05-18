using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Orders
{
    public class DTOOrderURequest : DTOOrderCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be greater than 0.")]
        public int OrderID { get; set; }
    }
}
