

using System.Reflection.Emit;

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOOrderResponse
    {
        public int ID { get; set; }
        public int TableID { get; set; }
        public int EmployeeID { get; set; }
        public int StatusOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public DTOTableResponse? tables { get; set; }
        public DTOEmployeeResponse? employees { get; set; }
        public DTOStatusOrderResponse? statusOrders { get; set; }
    }
}
