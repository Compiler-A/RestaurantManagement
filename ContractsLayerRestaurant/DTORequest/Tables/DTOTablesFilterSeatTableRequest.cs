using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Tables
{
    public class DTOTablesFilterSeatTableRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seats must be a positive integer.")]
        public int Seats { get; set; }
    }
}
