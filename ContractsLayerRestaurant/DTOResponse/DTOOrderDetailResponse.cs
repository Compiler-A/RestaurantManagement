

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOOrderDetailResponse
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public DTOOrderResponse? Order { get; set; }
        public DTOMenuItemResponse? Item { get; set; }
    }
}
