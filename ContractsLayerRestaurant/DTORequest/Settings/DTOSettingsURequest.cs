using System.ComponentModel.DataAnnotations;

namespace ContractsLayerRestaurant.DTORequest.Settings
{
    public class DTOSettingsURequest : DTOSettingsCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
        public DTOSettingsURequest(int id, string name, decimal value) : base(name, value)
        {
            ID = id;
        }
    }
}
