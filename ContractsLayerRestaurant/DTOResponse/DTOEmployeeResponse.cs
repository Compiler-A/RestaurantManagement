

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOEmployeeResponse : DTOPeopleResponse
    {
        public int ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
    }
}
