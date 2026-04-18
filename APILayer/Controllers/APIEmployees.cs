using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{

    [Authorize]
    [Route("api/Employees")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIEmployees : BaseController
    {
        private readonly IEmployeesService employees;
        public APIEmployees(IEmployeesService employees)
        {
            this.employees = employees;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(Name = "GetAllEmployeesAsync")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOEmployees>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var list = await employees.GetAllEmployeesAsync(page);
            return CreateResponse<IEnumerable<DTOEmployees>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("{ID}", Name = "GetEmployeeByID")]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var DTO = await employees.GetEmployeeAsync(ID);
            return CreateResponse<DTOEmployees>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(Name = "AddEmployee")]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> CreateAsync([FromBody] DTOEmployeesCRequest employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            

            var success = await this.employees.CreateEmployeeAsync(employee);
            return CreatedAtRoute("GetEmployeeByID", new { ID = success!.ID }, success);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(Name = "UpdateEmployee")]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> UpdateAsync([FromBody] DTOEmployeesURequest employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            

            var dto = await this.employees.UpdateEmployeeAsync(employee);
            return CreateResponse<DTOEmployees>(dto!, StatusCodes.Status200OK, "Employee Updated Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{ID}", Name = "DeleteEmployee")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var success = await employees.DeleteEmployeeAsync(ID);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Employee Deleted Successfully!");
        }


        [Authorize(Roles = "Manager")]
        [HttpPost("changed-password", Name = "ChangeEmployeePasswordAsync")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePasswordAsync([FromBody] DTOEmployeesChangedPassword Changed)
        {
            if (Changed == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var success = await employees.ChangePasswordAsync(Changed);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Password Changed Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("user-name/{userName}", Name = "GetEmployeeByUserName")]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> GetByUserNameAsync([FromRoute] string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Username is null or empty.");
            }
            var DTO = await employees.GetEmployeeAsync(userName);
            return CreateResponse<DTOEmployees>(DTO!, StatusCodes.Status200OK, "Found Successfully!");

        }


    }
}
