using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Tables;



namespace APILayer.Controllers
{
    [Route("api/Tables")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APITables :  BaseController
    {
        private readonly IBusinessTables dataTablesBusiness;

        public APITables(IBusinessTables TableBusiness) 
        {
            dataTablesBusiness = TableBusiness;
        }

        [HttpGet("all-nopagination", Name = "GetAllTablesNoPagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllNoPaginationAsync()
        {

            var data = await dataTablesBusiness.GetAllTablesAsync();
            return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Find {data.Count} Data!");
        }

        [HttpGet(Name = "GetAllTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetAllTablesAsync(page);
            return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Find {data.Count} in this page!");

        }
        
        [HttpGet("all-availables", Name = "GetAllTablesAvailables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllAvailablesAsync()
        {
            var data = await dataTablesBusiness.GetAllTablesAvailablesAsync();
            return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Find {data.Count} in this page!");

        }

        [HttpGet("allfilter-seats", Name = "GetAllFilterSeats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetlAllFilterSeatsAsync([FromQuery] DTOTablesFilterSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");

            
            var list = await dataTablesBusiness.GetTablesFilter2Async(Request);
            return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }


        [HttpGet("allfilter-statustables", Name = "GetAllMenuTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllFilterStatustablesAsync([FromQuery] DTOTablesFilterStatusTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetTablesFilter1Async(Request);
            return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [HttpGet("allfilter-global", Name = "GetAllFilterSeatsStatusTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllFilterSeatsStatusTablesAsync([FromQuery] DTOTablesFilterStatusAndSeatTableRequest Request)
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null!");


            var list = await dataTablesBusiness.GetTablesFilter3Async(Request);
            return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [HttpGet("table-name", Name = "GetTableByTableName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> GetByTableNameAsync([FromQuery] string tableNumber = "")
        {
            if (!string.IsNullOrWhiteSpace(tableNumber))
            {
                throw new ArgumentOutOfRangeException("Table Number is Empty!");
            }
            var list = await dataTablesBusiness.GetTableByNameAsync(tableNumber);
            return CreateResponse<DTOTables>(list!, StatusCodes.Status200OK, $"Ramadan N word");

        }


        [HttpGet("{ID}", Name = "GetTableByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var data = await dataTablesBusiness.GetTableAsync(ID);
            return CreateResponse<DTOTables>(data!, StatusCodes.Status200OK, "Found Data!");

        }

        [HttpPost(Name = "AddTable")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> CreateAsync(DTOTablesCRequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.AddTableAsync(Table);
            return CreatedAtRoute("GetTableByID", new { ID = dto!.ID }, dto);

        }

        [HttpPut(Name = "UpdateTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> UpdateAsync(DTOTablesURequest Table)
        {

            if (Table == null)
                throw new ArgumentNullException("Request is null!");


            var dto = await dataTablesBusiness.UpdateTableAsync(Table);
            return CreateResponse<DTOTables>(dto!, StatusCodes.Status200OK, "Update Saccessfully!");

        }


        [HttpDelete("{ID}" , Name = "DeleteTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var result = await dataTablesBusiness.DeleteTableAsync(ID);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "Delete Saccessfully!");

        }
    }
}
