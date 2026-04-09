using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusTables;



namespace APILayer.Controllers
{
    [Route("api/StatusTables")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIStatusTables : BaseController
    {

        private readonly IBusinessStatusTables _businessStatusTables;

        public APIStatusTables(IBusinessStatusTables businessStatusTables)
        {
            _businessStatusTables = businessStatusTables;
        }

        [HttpGet(Name = "GetAllStatusTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOStatusTables>>>> GetAllAsync([FromQuery] int Page)
        {
            if (Page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await _businessStatusTables.GetAllStatusTablesAsync(Page);
            return CreateResponse<IEnumerable<DTOStatusTables>>(list!, StatusCodes.Status200OK, "Completed!");

        }

        [HttpGet("{ID}", Name = "GetStatusTableByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var statusTable = await _businessStatusTables.GetStatusTableAsync(ID);
            return CreateResponse<DTOStatusTables>(statusTable!, StatusCodes.Status200OK, "Completed!");

        }


        [HttpPost(Name = "AddStatusTable")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> CreateAsync([FromBody] DTOStatusTablesCRequest Request)
        {

            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await _businessStatusTables.AddStatusTableAsync(Request);
            return CreatedAtRoute("GetStatusTableByID", new { ID = result!.ID }, result);

        }

        

        [HttpPut(Name = "UpdateStatusTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> UpdateAsync([FromBody] DTOStatusTablesURequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _businessStatusTables.UpdateStatusTableAsync(Request);
            return CreateResponse<DTOStatusTables>(result!, StatusCodes.Status200OK, "StatusTable updated successfully.");
            
        }


        [HttpDelete("{ID}", Name = "DeleteStatusTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await _businessStatusTables.DeleteStatusTableAsync(ID);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "StatusTable deleted successfully.");
        }
    }
}
