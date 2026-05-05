using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Employees
{
    public class DTOEmployeesURequest : DTOEmployeesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0")]
        public int ID { get; set; }

        public DTOEmployeesURequest(int ID, string Name, int JobID, string userName, string password)
            : base(Name, JobID, userName, password)
        {
            this.ID = ID;
        }

    }
}
