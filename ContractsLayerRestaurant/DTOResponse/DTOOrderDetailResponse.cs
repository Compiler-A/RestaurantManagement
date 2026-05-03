

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOOrderDetailResponse
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; } = string.Empty;
    }
}
