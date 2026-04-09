
using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.StatusMenus
{
    public class DTOStatusMenusURequest : DTOStatusMenusCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
        public DTOStatusMenusURequest(int ID, string Name, string? Description) : base(Name, Description)
        {
            this.ID = ID;
        }
    }
}
