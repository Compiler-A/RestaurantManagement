using DomainLayer.Entities;
using ContractsLayerRestaurant.DTOResponse;

namespace BusinessLayerRestaurant.Mapper
{
    public static class EmployeeMapper
    {
        public static DTOEmployeeResponse ToResponse(this Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            return new DTOEmployeeResponse
            {
                ID = employee.EmployeeID,
                Name = employee.Name,
                UserName = employee.Username,
                JobName = employee.JobRole?.JobName ?? string.Empty,
            };
        }
    }
}
