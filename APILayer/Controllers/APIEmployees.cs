using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/APIEmployees")]
    [ApiController]
    public class APIEmployees : BaseController
    {
        private readonly IBusinessEmployees employees;
        public APIEmployees(IBusinessEmployees employees)
        {
            this.employees = employees;
        }

        [HttpGet("GetAllEmployees", Name = "GetAllEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOEmployees>>>> GetAllEmployees([FromQuery] int page = 1)
        {
            try
            {
                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOEmployees>>(null!, 400, "Page number must be greater than 0.");
                }
                var list = await employees.GetAllEmployeesAsync(page);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOEmployees>>(null!, 404, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOEmployees>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOEmployees>>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("Login", Name = "LoginEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> LoginEmployee([FromBody] LoginRequest Login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Login.UserName) || string.IsNullOrWhiteSpace(Login.Password))
                {
                    return CreateResponse<DTOEmployees>(null!, 400, "Name or Password is null or empty.");
                }
                var DTO = await employees.LoginEmployeeAsync(Login.UserName, Login.Password);
                if (DTO == null)
                {
                    return CreateResponse<DTOEmployees>(null!, 401, "Not Found!");
                }
                return CreateResponse<DTOEmployees>(DTO, 200, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetEmployeeByID/{ID}", Name = "GetEmployeeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> GetEmployeeByID([FromRoute] int ID = 1)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOEmployees>(null!, 400, "ID <= 0.");
                }
                var DTO = await employees.GetEmployeeAsync(ID);
                if (DTO == null)
                {
                    return CreateResponse<DTOEmployees>(null!, 404, "Not Found!");
                }
                return CreateResponse<DTOEmployees>(DTO, 200, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetEmployeeByUserName/{userName}", Name = "GetEmployeeByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> GetEmployeeByUserName([FromRoute] string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return CreateResponse<DTOEmployees>(null!, 400, "Username is null or empty.");
                }
                var DTO = await employees.GetEmployeeAsync(userName);
                if (DTO == null)
                {
                    return CreateResponse<DTOEmployees>(null!, 404, "Not Found!");
                }
                return CreateResponse<DTOEmployees>(DTO, 200, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddEmployee", Name = "AddEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> AddEmployee([FromBody] DTOEmployees employee)
        {
            try
            {
                if (employee == null || employee.JobID < 0)
                {
                    return CreateResponse<DTOEmployees>(null!, 400, "Employee data is null.");
                }
                employees.Employees = employee;
                var success = await employees.Save();
                if (!success)
                {
                    return CreateResponse<DTOEmployees>(null!, 500, "Failed to add employee.");
                }
                return CreatedAtRoute(
                    "GetEmployeeByID",
                    new { ID = this.employees.Employees!.ID },
                    employees.Employees
                );
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("UpdateEmployee", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> UpdateEmployee([FromBody] DTOEmployees employee)
        {
            try
            {
                if (employee == null || employee.ID <= 0 || employee.JobID < 0)
                {
                    return CreateResponse<DTOEmployees>(null!, 400, "Invalid employee data.");
                }
                employees.Employees = await employees.GetEmployeeAsync(employee.ID);
                employees.Employees = employee;
                var success = await employees.Save();
                if (!success)
                {
                    return CreateResponse<DTOEmployees>(null!, 404, "Failed to update employee.");
                }
                return CreateResponse<DTOEmployees>(employees.Employees!, 200, "Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("DeleteEmployee/{ID}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEmployee([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<bool>(false, 400, "ID <= 0.");
                }
                var success = await employees.Delete(ID);
                if (!success)
                {
                    return CreateResponse<bool>(false, 404, "Failed to delete employee.");
                }
                return CreateResponse<bool>(true, 200, "Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<bool>(false, 500, "Internal server error: " + ex.Message);
            }
        }
    }
}
