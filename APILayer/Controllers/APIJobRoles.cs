using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.JobRoles;
using Microsoft.AspNetCore.Authorization;



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
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOJobRoles>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await jobRoles.GetAllJobRolesAsync(page);
            return CreateResponse<IEnumerable<DTOJobRoles>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }

        [AllowAnonymous]
        [HttpGet("{ID}", Name = "GetJobRoleByID")]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var DTO = await jobRoles.GetJobRoleAsync(ID);
            return CreateResponse<DTOJobRoles>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(Name = "AddJobRole")]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> CreateAsync([FromBody] DTOJobRolesCRequest JobRole)
        {
            if (JobRole == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await jobRoles.AddJobRoleAsync(JobRole);
            return CreatedAtRoute("GetJobRoleByID", new { ID = result!.ID }, result);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(Name = "UpdateJobRole")]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> UpdateAsync([FromBody] DTOJobRolesURequest Update)
        {
            if (Update == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await jobRoles.UpdateJobRoleAsync(Update);
            return CreateResponse<DTOJobRoles>(result!, StatusCodes.Status200OK, "Job Role Updated Successfully!");

           
        }

        [Authorize(Roles = "Manager")]
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

