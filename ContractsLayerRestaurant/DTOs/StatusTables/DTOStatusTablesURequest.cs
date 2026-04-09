
using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.StatusTables
{
    public class DTOStatusTablesURequest : DTOStatusTablesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int ID { get; set; }

        public DTOStatusTablesURequest(int ID, string Name) : base(Name)
        {
            this.ID = ID;
        }
    }
}
