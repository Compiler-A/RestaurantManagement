using ContractsLayerRestaurant.DTORequest.MenuItems;
using ContractsLayerRestaurant.DTORequest.Orders;


namespace ContractsLayerRestaurant.DTORequest.OrderDetails
{
    public class OrderDetail
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public Order? Order { get; set; }
        public MenuItem? Item { get; set; }
    }
}
