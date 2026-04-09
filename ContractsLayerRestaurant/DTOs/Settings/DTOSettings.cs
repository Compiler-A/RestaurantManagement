
namespace ContractsLayerRestaurant.DTOs.Settings
{
    public class DTOSettings
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }

        public DTOSettings()
        {
            ID = -1;
            Name = string.Empty;
            Value = 0;
        }
        public DTOSettings(int id, string key, string value)
        {
            ID = id;
            Name = key;
            Value = 0;
        }
    }
}
