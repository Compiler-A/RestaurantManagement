using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;



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
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("all-nopagination", Name = "GetAllTablesNoPagination")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Table>>>> GetAllNoPaginationAsync()
        {

            var data = await dataTablesBusiness.GetAllAsync();
            return CreateResponse<IEnumerable<Table>>(data, StatusCodes.Status200OK, $"Row: {data.Count}");
        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet(Name = "GetAllTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Table>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetAllAsync(page);
            return CreateResponse<IEnumerable<Table>>(data, StatusCodes.Status200OK, $"Row: {data.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("all-availables", Name = "GetAllTablesAvailables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Table>>>> GetAllAvailablesAsync()
        {
            var data = await dataTablesBusiness.GetAllAvailablesAsync();
            return CreateResponse<IEnumerable<Table>>(data, StatusCodes.Status200OK, $"Row: {data.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("allfilter-seats", Name = "GetAllFilterSeats")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Table>>>> GetlAllFilterSeatsAsync([FromQuery] DTOTablesFilterSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");

            
            var list = await dataTablesBusiness.GetFilter2Async(Request);
            return CreateResponse<IEnumerable<Table>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }


        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("allfilter-statustables", Name = "GetAllMenuTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Table>>>> GetAllFilterStatustablesAsync([FromQuery] DTOTablesFilterStatusTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetFilter1Async(Request);
            return CreateResponse<IEnumerable<Table>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("allfilter-global", Name = "GetAllFilterSeatsStatusTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Table>>>> GetAllFilterSeatsStatusTablesAsync([FromQuery] DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetFilter3Async(Request);
            return CreateResponse<IEnumerable<Table>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("table-name", Name = "GetTableByTableName")]
        public async Task<ActionResult<ApiResponse<Table>>> GetByTableNameAsync([FromQuery] string tableNumber = "")
        {
            if (!string.IsNullOrWhiteSpace(tableNumber))
            {
                throw new ArgumentOutOfRangeException("Table Number is Empty!");
            }
            var list = await dataTablesBusiness.GetByNameAsync(tableNumber);
            return CreateResponse<Table>(list!, StatusCodes.Status200OK, $"Found Successfully!");

        }


        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetTableByID")]
        public async Task<ActionResult<ApiResponse<Table>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetAsync(ID);
            return CreateResponse<Table>(data!, StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddTable")]
        public async Task<ActionResult<ApiResponse<Table>>> CreateAsync(DTOTablesCRequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.CreateAsync(Table);
            return CreatedAtRoute("GetTableByID", new { ID = dto!.ID }, dto);

        }

        [Authorize(Roles = "Manager,Cleaner")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateTable")]
        public async Task<ActionResult<ApiResponse<Table>>> UpdateAsync(DTOTablesURequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.UpdateAsync(Table);
            return CreateResponse<Table>(dto!, StatusCodes.Status200OK, "Table Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}" , Name = "DeleteTable")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var result = await dataTablesBusiness.DeleteAsync(ID);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "Table Deleted Successfully!");

        }
    }
}
