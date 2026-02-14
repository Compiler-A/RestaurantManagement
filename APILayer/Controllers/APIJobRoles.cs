using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/APIJobRoles")]
    [ApiController]
    public class APIJobRoles : BaseController
    {
        private readonly IBusinessJobRoles jobRoles;
        public APIJobRoles(IBusinessJobRoles jobRoles)
        {
            this.jobRoles = jobRoles;
        }

        [HttpGet("GetAllJobRoles", Name = "GetAllJobRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOJobRoles>>>> GetAllJobRoles([FromQuery] int page = 1)
        {
            try
            {
                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOJobRoles>>(null!, 400, "Page number must be greater than 0.");
                }
                var list = await jobRoles.GetAllJobRolesAsync(page);

                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOJobRoles>>(null!, 404, "Not Found!");
                }

                return CreateResponse<IEnumerable<DTOJobRoles>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOJobRoles>>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetJobRole/{ID}", Name = "GetJobRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> GetJobRole([FromRoute] int ID = 1)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOJobRoles>(null!, 400, "ID <= 0.");
                }
                var DTO = await jobRoles.GetJobRolesAsync(ID);

                if (DTO == null)
                {
                    return CreateResponse<DTOJobRoles>(null!, 404, "Not Found!");
                }

                return CreateResponse<DTOJobRoles>(DTO, 200, "Find Saccessfully!");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOJobRoles>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddJobRole", Name = "AddJobRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> AddJobRole([FromBody] DTOJobRoles JobRole)
        {
            try
            {
                if (JobRole == null)
                    return CreateResponse<DTOJobRoles>(null!, 400, "Body is null.");

                if (string.IsNullOrWhiteSpace(JobRole.Name))
                    return CreateResponse<DTOJobRoles>(null!, 400, "Name is required.");

                jobRoles.JobRoles = JobRole;

                var result = await jobRoles.Save();

                if (!result)
                    return CreateResponse<DTOJobRoles>(null!, 500, "Insert failed.");

                return CreatedAtRoute(
                    "GetJobRole",
                    new { ID = jobRoles.JobRoles!.ID },
                    jobRoles.JobRoles
                );
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOJobRoles>(null!, 500, ex.Message);
            }
        }

        [HttpPut("UpdateJobRole", Name = "UpdateJobRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> UpdateJobRole([FromBody] DTOJobRoles Update)
        {
            try
            {
                if (Update == null || Update.ID <= 0)
                {
                    return CreateResponse<DTOJobRoles>(null!, 400, "Bad Ruquest");
                }

                var existingStatusOrder = await jobRoles.GetJobRolesAsync(Update.ID);

                if (existingStatusOrder == null)
                {
                    return CreateResponse<DTOJobRoles>(null!, 404, "Job Role not found.");
                }
                jobRoles.JobRoles = Update;

                var result = await jobRoles.Save();
                return result ?
                    CreateResponse<DTOJobRoles>(jobRoles.JobRoles!, 200, "Job Role updated successfully.")
                    :
                    CreateResponse<DTOJobRoles>(null!, 500, "Failed to update JobRole.");

            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOJobRoles>(null!, 500, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("DeleteJobRole/{id}", Name = "DeleteJobRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOJobRoles>>> DeleteJobRole([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<DTOJobRoles>(null!, 400, "Bad Ruquest");
                }

                var result = await jobRoles.Delete(id);

                if (result)
                {
                    return CreateResponse<DTOJobRoles>(null!, 200, "jobRole deleted successfully.");
                }
                else
                {
                    return CreateResponse<DTOJobRoles>(null!, 404, "JobRole not found or failed to delete.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOJobRoles>(null!, 500, "Internal server error: " + ex.Message);
            }
        }
    }
}

