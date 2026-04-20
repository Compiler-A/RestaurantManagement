using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;



namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/Tables")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APITables :  BaseController
    {
        private readonly ITablesService dataTablesBusiness;

        public APITables(ITablesService TableBusiness) 
        {
            dataTablesBusiness = TableBusiness;
        }

        [AllowAnonymous]
        [EnableRateLimiting("GetAllLimiter")]
        [HttpGet("all-nopagination", Name = "GetAllTablesNoPagination")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllNoPaginationAsync()
        {

            var data = await dataTablesBusiness.GetAllTablesAsync();
            return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Row: {data.Count}");
        }

        [AllowAnonymous]
        [EnableRateLimiting("GetAllLimiter")]
        [HttpGet(Name = "GetAllTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetAllTablesAsync(page);
            return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Row: {data.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting("GetAllLimiter")]
        [HttpGet("all-availables", Name = "GetAllTablesAvailables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllAvailablesAsync()
        {
            var data = await dataTablesBusiness.GetAllTablesAvailablesAsync();
            return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Row: {data.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting("GetAllLimiter")]
        [HttpGet("allfilter-seats", Name = "GetAllFilterSeats")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetlAllFilterSeatsAsync([FromQuery] DTOTablesFilterSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");

            
            var list = await dataTablesBusiness.GetTablesFilter2Async(Request);
            return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }


        [AllowAnonymous]
        [EnableRateLimiting("GetAllLimiter")]
        [HttpGet("allfilter-statustables", Name = "GetAllMenuTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllFilterStatustablesAsync([FromQuery] DTOTablesFilterStatusTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetTablesFilter1Async(Request);
            return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting("GetAllLimiter")]
        [HttpGet("allfilter-global", Name = "GetAllFilterSeatsStatusTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllFilterSeatsStatusTablesAsync([FromQuery] DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetTablesFilter3Async(Request);
            return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting("GetOneLimiter")]
        [HttpGet("table-name", Name = "GetTableByTableName")]
        public async Task<ActionResult<ApiResponse<DTOTables>>> GetByTableNameAsync([FromQuery] string tableNumber = "")
        {
            if (!string.IsNullOrWhiteSpace(tableNumber))
            {
                throw new ArgumentOutOfRangeException("Table Number is Empty!");
            }
            var list = await dataTablesBusiness.GetTableByNameAsync(tableNumber);
            return CreateResponse<DTOTables>(list!, StatusCodes.Status200OK, $"Found Successfully!");

        }


        [AllowAnonymous]
        [EnableRateLimiting("GetOneLimiter")]
        [HttpGet("{ID}", Name = "GetTableByID")]
        public async Task<ActionResult<ApiResponse<DTOTables>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetTableAsync(ID);
            return CreateResponse<DTOTables>(data!, StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting("AddLimiter")]
        [HttpPost(Name = "AddTable")]
        public async Task<ActionResult<ApiResponse<DTOTables>>> CreateAsync(DTOTablesCRequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.AddTableAsync(Table);
            return CreatedAtRoute("GetTableByID", new { ID = dto!.ID }, dto);

        }

        [Authorize(Roles = "Manager,Cleaner")]
        [EnableRateLimiting("UpdateLimiter")]
        [HttpPut(Name = "UpdateTable")]
        public async Task<ActionResult<ApiResponse<DTOTables>>> UpdateAsync(DTOTablesURequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.UpdateTableAsync(Table);
            return CreateResponse<DTOTables>(dto!, StatusCodes.Status200OK, "Table Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting("DeleteLimiter")]
        [HttpDelete("{ID}" , Name = "DeleteTable")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var result = await dataTablesBusiness.DeleteTableAsync(ID);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "Table Deleted Successfully!");

        }
    }
}
