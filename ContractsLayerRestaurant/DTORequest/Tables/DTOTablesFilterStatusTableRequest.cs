
using System.ComponentModel.DataAnnotations;

namespace ContractsLayerRestaurant.DTORequest.Tables
{
    public class DTOTablesFilterStatusTableRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusTableID must be a positive integer.")]
        public int StatusTableID { get; set; }
    }
}
