using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTOs.Settings
{
    public class DTOSettingsCRequest
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public decimal Value { get; set; }

        public DTOSettingsCRequest(string name, decimal value)
        {
            Name = name;
            Value = value;
        }
    }
}
