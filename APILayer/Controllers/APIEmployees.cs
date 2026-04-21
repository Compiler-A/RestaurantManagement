using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Authentication;
using System.Security.Claims;

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
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
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
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> GetByIDAsync
            ([FromRoute] int ID , [FromServices] IAuthorizationService authorizationService)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var authResult = await authorizationService.AuthorizeAsync(
                User,
                ID,
                "EmployeeOwnerOrAdmin");

            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");

            var DTO = await employees.GetEmployeeAsync(ID);
            return CreateResponse<DTOEmployees>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(Name = "AddEmployee")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
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
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
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
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
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


        [Authorize]
        [EnableRateLimiting(NameRateLimitPolicies.Auth)]
        [HttpPost("changed-password", Name = "ChangeEmployeePasswordAsync")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePasswordAsync([FromBody] DTOEmployeesChangedPassword Changed)
        {
            if (Changed == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedEmployeeId) &&
                authenticatedEmployeeId != Changed.ID)
            {
                throw new AuthenticationException("Invalid credentials");
            }
            var success = await employees.ChangePasswordAsync(Changed);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Password Changed Successfully!");
        }

        [Authorize]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("user-name/{userName}", Name = "GetEmployeeByUserName")]
        public async Task<ActionResult<ApiResponse<DTOEmployees>>> GetByUserNameAsync
            ([FromRoute] string userName, [FromServices] IAuthorizationService authorizationService)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Username is null or empty.");
            }

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                userName,
                "EmployeeByUserNameOwnerOrAdmin");

            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");
            var DTO = await employees.GetEmployeeAsync(userName);
            return CreateResponse<DTOEmployees>(DTO!, StatusCodes.Status200OK, "Found Successfully!");

        }


    }
}
