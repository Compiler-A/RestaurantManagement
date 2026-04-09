using ContractsLayerRestaurant.DTOs.MenuItems;
using ContractsLayerRestaurant.DTOs.Orders;


namespace ContractsLayerRestaurant.DTOs.OrderDetails
{
    public class DTOOrderDetails
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public DTOOrders? Order { get; set; }
        public DTOMenuItems? Item { get; set; }

        public DTOOrderDetails()
        {
            ID = -1;
            OrderID = -1;
            ItemID = -1;
            Quantity = -1;
            SubTotal = -1;
        }
        public DTOOrderDetails(int iD, int orderID, int itemID, int quantity, decimal subTotal)
        {
            ID = iD;
            OrderID = orderID;
            ItemID = itemID;
            Quantity = quantity;
            SubTotal = subTotal;
        }
    }
}
