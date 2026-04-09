using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.OrderDetails
{
    public class DTOOrderDetailsURequest : DTOOrderDetailsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }

        public DTOOrderDetailsURequest
            (int iD, int orderID, int itemID, int quantity, decimal subTotal)
            : base(orderID, itemID, quantity, subTotal)
        {
            ID = iD;
            OrderID = orderID;
            ItemID = itemID;
            Quantity = quantity;
            SubTotal = subTotal;
        }
    }
}
