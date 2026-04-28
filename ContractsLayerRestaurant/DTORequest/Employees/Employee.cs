using ContractsLayerRestaurant.DTORequest.JobRoles;


namespace ContractsLayerRestaurant.DTORequest.Employees
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int JobID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public JobRole? JobRoles { get; set; }

    }
}
