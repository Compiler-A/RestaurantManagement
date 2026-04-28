using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.OrderDetails
{
    public class DTOOrderDetailsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be greater than 0.")]
        public int OrderID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ItemID must be greater than 0.")]
        public int ItemID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SubTotal must be greater than 0.")]
        public decimal SubTotal { get; set; }
        public DTOOrderDetailsCRequest(int orderID, int itemID, int quantity, decimal subTotal)
        {
            OrderID = orderID;
            ItemID = itemID;
            Quantity = quantity;
            SubTotal = subTotal;
        }
    }
}
