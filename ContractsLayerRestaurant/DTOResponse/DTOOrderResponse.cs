
namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOOrderResponse
    {
        public int ID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string tableNumber { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string statusOrderName { get; set; } = string.Empty;

        public List<DTOOrderDetailResponse> Details { get; set; } = new List<DTOOrderDetailResponse>();
    }
}
