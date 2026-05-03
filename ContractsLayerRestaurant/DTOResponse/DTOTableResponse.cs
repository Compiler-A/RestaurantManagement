

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOTableResponse
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Seats { get; set; }
        public string StatusTableName { get; set; } = string.Empty;
    }
}
