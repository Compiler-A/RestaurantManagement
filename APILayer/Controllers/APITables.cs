using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;


namespace APILayer.Controllers
{
    [Route("api/APITables")]
    [ApiController]
    public class APITables :  BaseController
    {
        private readonly IBusinessTables dataTablesBusiness;

        public APITables(IBusinessTables TableBusiness) 
        {
            dataTablesBusiness = TableBusiness;
        }

        [HttpGet("GetAllTablesNoPagination", Name = "GetAllTablesNoPagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllTablesNoPagination()
        {
            try
            {
                var data = await dataTablesBusiness.GetAllTablesAsync();
                if (data.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status404NotFound, "Not Found Data!");
                }
                else
                {
                    return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Find {data.Count} Data!");
                }
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetAllTables", Name = "GetAllTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllTables([FromQuery] int page = 1)
        {
            try
            {
                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }
                var data = await dataTablesBusiness.GetAllTablesAsync(page);
                if (data.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status404NotFound, "Not Found Data in this Page!");
                }
                else
                {
                    return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Find {data.Count} in this page!");
                }
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
        
        [HttpGet("GetAllTablesAvailables", Name = "GetAllTablesAvailables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllTablesAvailables()
        {
            try
            {
                var data = await dataTablesBusiness.GetAllTablesAvailablesAsync();
                if (data.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status404NotFound, "Not Found Data in this Page!");
                }
                else
                {
                    return CreateResponse<IEnumerable<DTOTables>>(data, StatusCodes.Status200OK, $"Find {data.Count} in this page!");
                }
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetAllFilterSeatsTable", Name = "GetAllFilterSeatsTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllFilterSeatsTables([FromQuery] int page = 1, [FromQuery] int Seats = 2)
        {
            try
            {
                if (page <= 0 || Seats <= 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!,StatusCodes.Status400BadRequest, "Page or Seats number must be greater than 0.");
                }
                var list = await dataTablesBusiness.GetTablesFilter2Async(page,Seats);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("GetAllFilterStatusTables", Name = "GetAllMenuTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllMenuTables([FromQuery] int page = 1, [FromQuery] int StatusTable = 1)
        {
            try
            {
                if (page <= 0 || StatusTable <= 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status400BadRequest, "Page or Status Table number must be greater than 0.");
                }
                var list = await dataTablesBusiness.GetTablesFilter1Async(page, StatusTable);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status200OK, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("GetAllFilterSeatsStatusTables", Name = "GetAllFilterSeatsStatusTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTables>>>> GetAllFilterSeatsStatusTables([FromQuery] int page = 1, [FromQuery] int StatusTable = 1, [FromQuery] int Seats = 2)
        {
            try
            {
                if (page <= 0 || StatusTable < -1 || Seats < -1)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status400BadRequest, "Page or Status Table number or Seats number must be greater than 0.");
                }
                var list = await dataTablesBusiness.GetTablesFilter3Async(page, StatusTable, Seats);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOTables>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetTableByTableName", Name = "GetTableByTableName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> GetTableByTableName([FromQuery] string tableNumber = "")
        {
            try
            {
                var list = await dataTablesBusiness.GetTableByNameAsync(tableNumber);
                if (list == null)
                {
                    return CreateResponse<DTOTables>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<DTOTables>(list, StatusCodes.Status200OK, $"Ramadan N word");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet("GetTableByID/{ID}", Name = "GetTableByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> GetTableByID([FromRoute] int ID = 1)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOTables>(null!, StatusCodes.Status400BadRequest, "Bad Value");
                }
                var data = await dataTablesBusiness.GetTableAsync(ID);
                if (data == null)
                {
                    return CreateResponse<DTOTables>(null!, StatusCodes.Status404NotFound, "Not Found Data!");
                }
                return CreateResponse<DTOTables>(data, StatusCodes.Status200OK, "Found Data!");

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddTable")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> AddTables(DTOTablesCRequest Table)
        {
            try
            {
                if (Table == null || Table.StatusTableID <= 0)
                {
                    return CreateResponse<DTOTables>(null!, StatusCodes.Status400BadRequest, "Bad Value");
                }

                var dto = await dataTablesBusiness.AddTableAsync(Table);

                if (dto != null)
                {
                    return CreateResponse<DTOTables>(dto!, StatusCodes.Status200OK, "Added Saccessfully.");
                }
                return CreateResponse<DTOTables>(null!, StatusCodes.Status500InternalServerError, "A problem happened while handling your request.");

            }

            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("UpdateTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> UpdateTable(DTOTablesURequest Table)
        {
            try
            {
                if (Table.ID <= 0 || Table.StatusTableID <= 0)
                {
                    return CreateResponse<DTOTables>(null!, StatusCodes.Status400BadRequest, "Bad Value");
                }
                
                var dto = await dataTablesBusiness.UpdateTableAsync(Table);
                if (dto != null)
                {
                    return CreateResponse<DTOTables>(dto!, StatusCodes.Status200OK, "Update Saccessfully!");

                }
                return CreateResponse<DTOTables>(null!, StatusCodes.Status500InternalServerError,  "A problem happened while handling your request.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("DeleteTable/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> DeleteTable(int ID)
        {
            try
            {
                if (ID <=0)
                {
                    return CreateResponse<DTOTables>(null!, StatusCodes.Status400BadRequest, "Bad Value");
                }
                if (await dataTablesBusiness.DeleteTableAsync(ID))
                {
                    return CreateResponse<DTOTables>(null!, StatusCodes.Status200OK, "Delete Saccessfully!");
                }
                return CreateResponse<DTOTables>(null!, StatusCodes.Status404NotFound, "A problem happened while handling your request.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}
