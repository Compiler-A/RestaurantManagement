

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOTableResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public int StatusTableID { get; set; }
        public DTOStatusTableResponse? StatusTable { get; set; }
    }
}
