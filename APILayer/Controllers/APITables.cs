using APILayer.Filters;
using ContractsLayerRestaurant.Interfaces.Services;
using BusinessLayerRestaurant.Mapper;
using ContractsLayerRestaurant.DTORequest.Tables;
using ContractsLayerRestaurant.DTOResponse;
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
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("all-nopagination", Name = "GetAllTablesNoPagination")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTableResponse>>>> GetAllNoPaginationAsync()
        {

            var data = await dataTablesBusiness.GetAllAsync();
            var listResponse = data.Select( x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOTableResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");
        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet(Name = "GetAllTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTableResponse>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetAllAsync(page);
            var listResponse = data.Select(x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOTableResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("all-availables", Name = "GetAllTablesAvailables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTableResponse>>>> GetAllAvailablesAsync()
        {
            var data = await dataTablesBusiness.GetAllAvailablesAsync();
            var listResponse = data.Select(x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOTableResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("allfilter-seats", Name = "GetAllFilterSeats")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTableResponse>>>> GetlAllFilterSeatsAsync([FromQuery] DTOTablesFilterSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");

            
            var list = await dataTablesBusiness.GetFilter2Async(Request);
            var listResponse = list.Select(x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOTableResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }


        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("allfilter-statustables", Name = "GetAllMenuTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTableResponse>>>> GetAllFilterStatustablesAsync([FromQuery] DTOTablesFilterStatusTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetFilter1Async(Request);
            var listResponse = list.Select(x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOTableResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet("allfilter-global", Name = "GetAllFilterSeatsStatusTables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTableResponse>>>> GetAllFilterSeatsStatusTablesAsync([FromQuery] DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetFilter3Async(Request);
            var listResponse = list.Select(x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOTableResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("table-name", Name = "GetTableByTableName")]
        public async Task<ActionResult<ApiResponse<DTOTableResponse>>> GetByTableNameAsync([FromQuery] string tableNumber = "")
        {
            if (string.IsNullOrWhiteSpace(tableNumber))
            {
                throw new ArgumentOutOfRangeException("Table Number is Empty!");
            }
            var table = await dataTablesBusiness.GetByNameAsync(tableNumber);
            return CreateResponse<DTOTableResponse>(table!.ToResponse(), StatusCodes.Status200OK, $"Found Successfully!");

        }


        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetTableByID")]
        public async Task<ActionResult<ApiResponse<DTOTableResponse>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetAsync(ID);
            return CreateResponse<DTOTableResponse>(data!.ToResponse(), StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddTable")]
        public async Task<ActionResult<ApiResponse<DTOTableResponse>>> CreateAsync(DTOTablesCRequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.CreateAsync(Table);
            return CreatedAtRoute("GetTableByID", new { ID = dto!.TableID }, dto.ToResponse());
        }

        [Authorize(Roles = $"{RoleNames.Manager},{RoleNames.Cleaner}")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateTable")]
        public async Task<ActionResult<ApiResponse<DTOTableResponse>>> UpdateAsync(DTOTablesURequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.UpdateAsync(Table);
            return CreateResponse<DTOTableResponse>(dto!.ToResponse(), StatusCodes.Status200OK, "Table Updated Successfully!");

        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}" , Name = "DeleteTable")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var result = await dataTablesBusiness.DeleteAsync(ID);
            return CreateResponse<bool>(result, StatusCodes.Status200OK, "Table Deleted Successfully!");

        }
    }
}
