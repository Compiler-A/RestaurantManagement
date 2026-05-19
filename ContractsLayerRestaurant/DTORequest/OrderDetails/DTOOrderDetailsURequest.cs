using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.OrderDetails
{
    public class DTOOrderDetailsURequest : DTOOrderDetailsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SubTotal must be greater than 0.")]
        public decimal SubTotal { get; set; }
    }
}
