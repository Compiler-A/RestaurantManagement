using ContractsLayerRestaurant.DTOs.JobRoles;


namespace ContractsLayerRestaurant.DTOs.Employees
{
    public class DTOEmployees
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int JobID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DTOJobRoles? JobRoles { get; set; }

        public DTOEmployees()
        {
            ID = -1;
            Name = string.Empty;
            JobID = -1;
            UserName = string.Empty;
            Password = string.Empty;
        }
        public DTOEmployees(int id, string name, int jobID, string userName, string password)
        {
            ID = id;
            Name = name;
            JobID = jobID;
            UserName = userName;
            Password = password;
        }
    }
}
