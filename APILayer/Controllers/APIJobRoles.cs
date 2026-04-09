using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.JobRoles;



namespace APILayer.Controllers
{
    [Route("api/JobRoles")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIJobRoles : BaseController
    {
        private readonly IBusinessJobRoles jobRoles;
        public APIJobRoles(IBusinessJobRoles jobRoles)
        {
            this.jobRoles = jobRoles;
        }

        [HttpGet(Name = "GetAllJobRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOJobRoles>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await jobRoles.GetAllJobRolesAsync(page);
            return CreateResponse<IEnumerable<DTOJobRoles>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }

        [HttpGet("{ID}",Name = "GetJobRoleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var DTO = await jobRoles.GetJobRoleAsync(ID);
            return CreateResponse<DTOJobRoles>(DTO!, StatusCodes.Status200OK, "Find Saccessfully!");
        }

        [HttpPost(Name = "AddJobRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> CreateAsync([FromBody] DTOJobRolesCRequest JobRole)
        {
            if (JobRole == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await jobRoles.AddJobRoleAsync(JobRole);
            return CreatedAtRoute("GetJobRoleByID", new { ID = result!.ID }, result);
        }

        [HttpPut(Name = "UpdateJobRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> UpdateAsync([FromBody] DTOJobRolesURequest Update)
        {
            if (Update == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await jobRoles.UpdateJobRoleAsync(Update);
            return CreateResponse<DTOJobRoles>(result!, StatusCodes.Status200OK, "Job Role updated successfully.");

           
        }


        [HttpDelete("{id}", Name = "DeleteJobRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await jobRoles.DeleteJobRoleAsync(id);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "jobRole deleted successfully.");
        }
    }
}

