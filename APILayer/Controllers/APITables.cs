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
        private readonly BusinessLayerRestaurant.IDataTablesBusiness dataTablesBusiness;

        public APITables(IDataTablesBusiness TableBusiness) 
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
                var data = await dataTablesBusiness.GetAll();
                if (data.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 404, "Not Found Data!");
                }
                else
                {
                    return CreateResponse<IEnumerable<DTOTables>>(data, 200, $"Find {data.Count} Data!");
                }
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 400, "Bad Ruquest");
                }
                var data = await dataTablesBusiness.GetAll(page);
                if (data.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 404, "Not Found Data in this Page!");
                }
                else
                {
                    return CreateResponse<IEnumerable<DTOTables>>(data, 200, $"Find {data.Count} in this page!");
                }
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, 500, "Internal server error: " + ex.Message);
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
                var data = await dataTablesBusiness.GetAllTablesAvailables();
                if (data.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 404, "Not Found Data in this Page!");
                }
                else
                {
                    return CreateResponse<IEnumerable<DTOTables>>(data, 200, $"Find {data.Count} in this page!");
                }
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 400, "Page or Seats number must be greater than 0.");
                }
                var list = await dataTablesBusiness.GetFilterTables(page,Seats);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 404, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOTables>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 400, "Page or Status Table number must be greater than 0.");
                }
                var list = await dataTablesBusiness.GetMenuTables(page, StatusTable);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 404, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOTables>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 400, "Page or Status Table number or Seats number must be greater than 0.");
                }
                var list = await dataTablesBusiness.GetTableWithFilteringData(page, StatusTable, Seats);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOTables>>(null!, 404, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOTables>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTables>>(null!, 500, "Internal server error: " + ex.Message);
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
                var list = await dataTablesBusiness.LoadByTableNumber(tableNumber);
                if (list == null)
                {
                    return CreateResponse<DTOTables>(null!, 404, "Not Found!");
                }
                return CreateResponse<DTOTables>(list, 200, $"Ramadan N word");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<DTOTables>(null!, 400, "Bad Value");
                }
                var data = await dataTablesBusiness.LoadByID(ID);
                if (data == null)
                {
                    return CreateResponse<DTOTables>(null!, 404, "Not Found Data!");
                }
                return CreateResponse<DTOTables>(data, 200, "Found Data!");

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddTable")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> AddTables(DTOTables Table)
        {
            try
            {
                if (Table.ID <= 0 || Table.StatusTableID <= 0)
                {
                    return CreateResponse<DTOTables>(null!, 400, "Bad Value");
                }
                if (!await dataTablesBusiness.IsFindStatus(Table.StatusTableID))
                {
                    return CreateResponse<DTOTables>(null!, 404, "Not Found Status");
                }
                clsBusinessTables table = new clsBusinessTables(Table);
                if (await table.Save())
                {
                    return CreatedAtAction(nameof(GetTableByID), new { id = table.DTOTables!.ID }, table.DTOTables);
                }
                return CreateResponse<DTOTables>(null!, 500, "A problem happened while handling your request.");

            }

            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("UpdateTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTables>>> UpdateTable(DTOTables Table)
        {
            try
            {
                if (Table.ID <= 0 || Table.StatusTableID <= 0)
                {
                    return CreateResponse<DTOTables>(null!, 400, "Bad Value");
                }
                if (!await dataTablesBusiness.IsFindStatus(Table.StatusTableID))
                { 
                    return CreateResponse<DTOTables>(null!, 404, "Not Found Status");

                }
                clsBusinessTables t = new clsBusinessTables(Table, clsBusinessTables.enMode.Update);
                if (await t.Save())
                {
                    return CreateResponse<DTOTables>(t.DTOTables!, 200, "Update Saccessfully!");

                }
                return CreateResponse<DTOTables>(null!, 500,  "A problem happened while handling your request.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<DTOTables>(null!, 400, "Bad Value");
                }
                if (await dataTablesBusiness.Delete(ID))
                {
                    return CreateResponse<DTOTables>(null!, 200, "Delete Saccessfully!");
                }
                return CreateResponse<DTOTables>(null!, 404, "A problem happened while handling your request.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOTables>(null!, 500, "Internal server error: " + ex.Message);
            }
        }
    }
}
