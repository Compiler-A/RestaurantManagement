

namespace ContractsLayerRestaurant.DTOResponse
{
    public class DTOEmployeeResponse
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int JobID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DTOJobRoleResponse? JobRoles { get; set; }
    }
}
