
using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.StatusOrders
{
    public class DTOStatusOrdersCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public DTOStatusOrdersCRequest(string statusName)
        {
            this.Name = statusName;
        }
    }
}
