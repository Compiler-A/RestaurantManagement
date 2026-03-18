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
                    return CreateResponse<IEnumerable<DTOEmployees>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");
                }
                var list = await employees.GetAllEmployeesAsync(page);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOEmployees>>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOEmployees>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOEmployees>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
       
        [HttpPost("Login", Name = "LoginEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> LoginEmployee([FromBody] DTOEmployeesLoginRequest Login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Login.UserName) || string.IsNullOrWhiteSpace(Login.Password))
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status400BadRequest, "Name or Password is null or empty.");
                }
                var DTO = await employees.GetLoginEmployeeAsync(Login);
                if (DTO == null)
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<DTOEmployees>(DTO, StatusCodes.Status200OK, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("ChangedPassword", Name = "ChangeEmployeePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<bool>>> ChangeEmployeePassword([FromBody] DTOEmployeesChangedPassword Changed)
        {
            try
            {
                if (Changed == null || Changed.ID <= 0 || string.IsNullOrWhiteSpace(Changed.NewPassword))
                {
                    return CreateResponse<bool>(false, StatusCodes.Status400BadRequest, "Invalid password change request.");
                }
                var success = await employees.ChangePasswordAsync(Changed);
                if (!success)
                {
                    return CreateResponse<bool>(false, StatusCodes.Status404NotFound, "Failed to change password.");
                }
                return CreateResponse<bool>(true, StatusCodes.Status200OK, "Password changed successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<bool>(false, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
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
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var DTO = await employees.GetEmployeeAsync(ID);
                if (DTO == null)
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<DTOEmployees>(DTO, StatusCodes.Status200OK, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
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
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status400BadRequest, "Username is null or empty.");
                }
                var DTO = await employees.GetEmployeeAsync(userName);
                if (DTO == null)
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<DTOEmployees>(DTO, StatusCodes.Status200OK, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddEmployee", Name = "AddEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> AddEmployee([FromBody] DTOEmployeesCRequest employee)
        {
            try
            {
                if (employee == null || employee.JobID < 0)
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status400BadRequest, "Employee data is null.");
                }
                var success = await this.employees.CreateEmployeeAsync(employee);
                if (success == null)
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status500InternalServerError, "Failed to add employee.");
                }
                return CreateResponse<DTOEmployees>(success!, StatusCodes.Status200OK, "Failed to add employee.");

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("UpdateEmployee", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> UpdateEmployee([FromBody] DTOEmployeesURequest employee)
        {
            try
            {
                if (employee == null || employee.ID <= 0 || employee.JobID < 0)
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status400BadRequest, "Invalid employee data.");
                }
                var dto = await this.employees.UpdateEmployeeAsync(employee);
                if (!(dto != null))
                {
                    return CreateResponse<DTOEmployees>(null!, StatusCodes.Status404NotFound, "Failed to update employee.");
                }
                return CreateResponse<DTOEmployees>(dto!, StatusCodes.Status200OK, "Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOEmployees>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
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
                    return CreateResponse<bool>(false, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var success = await employees.DeleteEmployeeAsync(ID);
                if (!success)
                {
                    return CreateResponse<bool>(false, StatusCodes.Status404NotFound, "Failed to delete employee.");
                }
                return CreateResponse<bool>(true, StatusCodes.Status200OK, "Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<bool>(false, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}
