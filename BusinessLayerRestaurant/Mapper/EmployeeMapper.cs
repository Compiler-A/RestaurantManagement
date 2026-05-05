using DomainLayer.Entities;
using ContractsLayerRestaurant.DTOResponse;
using ContractsLayerRestaurant.DTORequest.Employees;

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
                ID = employee.ID,
                Name = employee.Name,
                UserName = employee.UserName,
                JobName = employee.JobRoles?.Name ?? string.Empty,
            };
        }
    }
}
