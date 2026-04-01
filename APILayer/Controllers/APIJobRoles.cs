using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace APILayer.Controllers
{
    [Route("api/JobRoles")]
    [ApiController]
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
            try
            {
                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOJobRoles>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");
                }
                var list = await jobRoles.GetAllJobRolesAsync(page);

                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOJobRoles>>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }

                return CreateResponse<IEnumerable<DTOJobRoles>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOJobRoles>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{ID}",Name = "GetJobRoleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var DTO = await jobRoles.GetJobRoleAsync(ID);

                if (DTO == null)
                {
                    return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }

                return CreateResponse<DTOJobRoles>(DTO, StatusCodes.Status200OK, "Find Saccessfully!");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost(Name = "AddJobRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> CreateAsync([FromBody] DTOJobRolesCRequest JobRole)
        {
            try
            {
                if (JobRole == null)
                    return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status400BadRequest, "Body is null.");

                if (string.IsNullOrWhiteSpace(JobRole.Name))
                    return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status400BadRequest, "Name is required.");


                var result = await jobRoles.AddJobRoleAsync(JobRole);

                if (result != null)
                    return CreatedAtRoute("GetJobRoleByID", new { ID = result.ID }, result);

                return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status500InternalServerError, "Insert failed.");

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut(Name = "UpdateJobRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> UpdateAsync([FromBody] DTOJobRolesURequest Update)
        {
            try
            {
                if (Update == null || Update.ID <= 0)
                {
                    return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }

                var existingStatusOrder = await jobRoles.GetJobRoleAsync(Update.ID);

                if (existingStatusOrder == null)
                {
                    return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status404NotFound, "Job Role not found.");
                }

                var result = await jobRoles.UpdateJobRoleAsync(Update);
                return result != null ?
                    CreateResponse<DTOJobRoles>(result, StatusCodes.Status200OK, "Job Role updated successfully.")
                    :
                    CreateResponse<DTOJobRoles>(null!, StatusCodes.Status500InternalServerError, "Failed to update JobRole.");

            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOJobRoles>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("{id}", Name = "DeleteJobRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<bool>(false, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }

                var result = await jobRoles.DeleteJobRoleAsync(id);

                if (result)
                {
                    return CreateResponse<bool>(true!, StatusCodes.Status200OK, "jobRole deleted successfully.");
                }
                else
                {
                    return CreateResponse<bool>(false!, StatusCodes.Status404NotFound, "JobRole not found or failed to delete.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<bool>(false!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}

