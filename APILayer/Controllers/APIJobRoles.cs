using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.JobRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;



namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/JobRoles")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIJobRoles : BaseController
    {
        private readonly IJobRolesService jobRoles;
        public APIJobRoles(IJobRolesService jobRoles)
        {
            this.jobRoles = jobRoles;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetAllJobRoles")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<JobRole>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await jobRoles.GetAllJobRolesAsync(page);
            return CreateResponse<IEnumerable<JobRole>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetJobRoleByID")]
        public async Task<ActionResult<ApiResponse<JobRole>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var DTO = await jobRoles.GetJobRoleAsync(ID);
            return CreateResponse<JobRole>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddJobRole")]
        public async Task<ActionResult<ApiResponse<JobRole>>> CreateAsync([FromBody] DTOJobRolesCRequest JobRole)
        {
            if (JobRole == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await jobRoles.AddJobRoleAsync(JobRole);
            return CreatedAtRoute("GetJobRoleByID", new { ID = result!.ID }, result);
        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateJobRole")]
        public async Task<ActionResult<ApiResponse<JobRole>>> UpdateAsync([FromBody] DTOJobRolesURequest Update)
        {
            if (Update == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await jobRoles.UpdateJobRoleAsync(Update);
            return CreateResponse<JobRole>(result!, StatusCodes.Status200OK, "Job Role Updated Successfully!");

           
        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{id}", Name = "DeleteJobRole")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await jobRoles.DeleteJobRoleAsync(id);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "Job Role Deleted Successfully!");
        }
    }
}

