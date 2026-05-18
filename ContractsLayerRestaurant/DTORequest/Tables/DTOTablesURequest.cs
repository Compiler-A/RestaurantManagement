
using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Tables
{
    public class DTOTablesURequest : DTOTablesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int ID { get; set; }
    }
}
