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

        [HttpGet(Name = "GetAllEmployeesAsync")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOEmployees>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var list = await employees.GetAllEmployeesAsync(page);
            return CreateResponse<IEnumerable<DTOEmployees>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }


        [HttpGet("{ID}", Name = "GetEmployeeByID")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var DTO = await employees.GetEmployeeAsync(ID);
            return CreateResponse<DTOEmployees>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [HttpPost(Name = "AddEmployee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> CreateAsync([FromBody] DTOEmployeesCRequest employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            

            var success = await this.employees.CreateEmployeeAsync(employee);
            return CreatedAtRoute("GetEmployeeByID", new { ID = success!.ID }, success);
        }

        [HttpPut(Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> UpdateAsync([FromBody] DTOEmployeesURequest employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            

            var dto = await this.employees.UpdateEmployeeAsync(employee);
            return CreateResponse<DTOEmployees>(dto!, StatusCodes.Status200OK, "Employee Updated Successfully!");
        }

        [HttpDelete("{ID}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var success = await employees.DeleteEmployeeAsync(ID);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Employee Deleted Successfully!");
        }


        [HttpPost("changed-password", Name = "ChangeEmployeePasswordAsync")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePasswordAsync([FromBody] DTOEmployeesChangedPassword Changed)
        {
            if (Changed == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var success = await employees.ChangePasswordAsync(Changed);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Password Changed Successfully!");

            
        }


        [HttpGet("user-name/{userName}", Name = "GetEmployeeByUserName")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
