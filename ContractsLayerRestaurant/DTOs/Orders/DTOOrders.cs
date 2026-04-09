using ContractsLayerRestaurant.DTOs.Employees;
using ContractsLayerRestaurant.DTOs.StatusOrders;
using ContractsLayerRestaurant.DTOs.Tables;

namespace ContractsLayerRestaurant.DTOs.Orders
{
    public class DTOOrders
    {
        public int ID { get; set; }
        public int TableID { get; set; }
        public int EmployerID { get; set; }
        public int StatusOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }


        public DTOTables? tables { get; set; }
        public DTOEmployees? employees { get; set; }
        public DTOStatusOrders? statusOrders { get; set; }


        public DTOOrders(int iD, int tableID, int employerID, int statusOrderID, DateTime orderDate, decimal? totalAmount)
        {
            ID = iD;
            TableID = tableID;
            EmployerID = employerID;
            StatusOrderID = statusOrderID;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
        }
        public DTOOrders()
        {
            ID = -1;
            TableID = -1;
            EmployerID = -1;
            StatusOrderID = -1;
            OrderDate = DateTime.MinValue;
            TotalAmount = null;
        }
    }



}
