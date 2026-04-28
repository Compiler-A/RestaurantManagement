using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Orders
{

    public class DTOOrderFilterRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int Page { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "TableID must be non-negative.")]
        public int TableID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "EmployeeID must be non-negative.")]
        public int EmployeeID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "StatusOrderID must be non-negative.")]
        public int StatusOrderID { get; set; }
    }

}
