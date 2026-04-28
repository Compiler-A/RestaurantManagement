
using System.ComponentModel.DataAnnotations;

namespace ContractsLayerRestaurant.DTORequest.TypeItems
{
    public class DTOTypeItemsURequest : DTOTypeItemsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int ID { get; set; }

        public DTOTypeItemsURequest(int id, string name, string? description)
            : base(name, description)
        {
            ID = id;
        }
    }
}
