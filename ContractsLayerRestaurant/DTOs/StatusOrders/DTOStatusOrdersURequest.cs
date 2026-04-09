
using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.StatusOrders
{
    public class DTOStatusOrdersURequest : DTOStatusOrdersCRequest
    {

        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }

        public DTOStatusOrdersURequest(int ID, string statusOrderName) : base(statusOrderName)
        {
            this.ID = ID;
        }
    }
}
