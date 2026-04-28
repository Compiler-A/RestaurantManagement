using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;



namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/StatusTables")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIStatusTables : BaseController
    {

        private readonly IStatusTablesService _businessStatusTables;

        public APIStatusTables(IStatusTablesService businessStatusTables)
        {
            _businessStatusTables = businessStatusTables;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetAllStatusTables")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StatusTable>>>> GetAllAsync([FromQuery] int Page)
        {
            if (Page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await _businessStatusTables.GetAllStatusTablesAsync(Page);
            return CreateResponse<IEnumerable<StatusTable>>(list!, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetStatusTableByID")]
        public async Task<ActionResult<ApiResponse<StatusTable>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var statusTable = await _businessStatusTables.GetStatusTableAsync(ID);
            return CreateResponse<StatusTable>(statusTable!, StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddStatusTable")]
        public async Task<ActionResult<ApiResponse<StatusTable>>> CreateAsync([FromBody] DTOStatusTablesCRequest Request)
        {

            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await _businessStatusTables.AddStatusTableAsync(Request);
            return CreatedAtRoute("GetStatusTableByID", new { ID = result!.ID }, result);

        }


        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateStatusTable")]
        public async Task<ActionResult<ApiResponse<StatusTable>>> UpdateAsync([FromBody] DTOStatusTablesURequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _businessStatusTables.UpdateStatusTableAsync(Request);
            return CreateResponse<StatusTable>(result!, StatusCodes.Status200OK, "Status Table Updated Successfully!");
            
        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}", Name = "DeleteStatusTable")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await _businessStatusTables.DeleteStatusTableAsync(ID);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "Status Table Deleted Successfully!");
        }
    }
}
