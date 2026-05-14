
namespace DomainLayer.Entities
{
    public class Setting
    {
        public int SettingID { get; set; }
        public string Name { get; set; } = null!;
        public decimal Value { get; set; }

    }
}
