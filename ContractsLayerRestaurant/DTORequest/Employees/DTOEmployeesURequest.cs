using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Employees
{
    public class DTOEmployeesURequest : DTOEmployeesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0")]
        public int ID { get; set; }
    }
}
