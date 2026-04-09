using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.Orders
{
    public class DTOOrderCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "TableID must be greater than 0.")]
        public int TableID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "EmployerID must be greater than 0.")]
        public int EmployerID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusOrderID must be greater than 0.")]
        public int StatusOrderID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be non-negative.")]
        public decimal? TotalAmount { get; set; }
        public DTOOrderCRequest(int tableID, int employerID, int statusOrderID, DateTime orderDate, decimal? totalAmount)
        {
            TableID = tableID;
            EmployerID = employerID;
            StatusOrderID = statusOrderID;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
        }
    }
}
