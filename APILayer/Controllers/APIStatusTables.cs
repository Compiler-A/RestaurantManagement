using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;


namespace APILayer.Controllers
{
    [Route("api/StatusTables")]
    [ApiController]
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
            try
            {
                if (Page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOStatusTables>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than zero.");
                }

                var list = await _businessStatusTables.GetAllStatusTablesAsync(Page);
                if (list == null || list.Count == 0)
                    return CreateResponse<IEnumerable<DTOStatusTables>>(null!, StatusCodes.Status404NotFound, "No Status Tables found.");

                return CreateResponse<IEnumerable<DTOStatusTables>>(list!, StatusCodes.Status200OK, "Completed!");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<IEnumerable<DTOStatusTables>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{ID}", Name = "GetStatusTableByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> GetByIDAsync([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }
                var statusTable = await _businessStatusTables.GetStatusTableAsync(ID);
                if (statusTable == null)
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status404NotFound, "StatusTable not found.");

                
                return CreateResponse<DTOStatusTables>(statusTable!, StatusCodes.Status200OK, "Completed!");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost(Name = "AddStatusTable")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> CreateAsync([FromBody] DTOStatusTablesCRequest Request)
        {
            try
            {
                if (Request == null)
                {

                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }

                _businessStatusTables.CreateRequest = Request;
                var result = await _businessStatusTables.AddStatusTableAsync(_businessStatusTables.CreateRequest);
                if (result != null)
                {
                    return CreatedAtRoute("GetStatusTableByID", new { ID = result.ID}, result);
                }
                else
                {
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status404NotFound, "Failed to add StatusTable.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        

        [HttpPut(Name = "UpdateStatusTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> UpdateAsync([FromBody] DTOStatusTablesURequest Request)
        {
            try
            {
                if (Request == null || Request.ID <= 0)
                {
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }

                

                if (await _businessStatusTables.isFindStatusTableAsync(Request.ID))
                {
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status404NotFound, "StatusTable not found.");
                }

                _businessStatusTables.UpdateRequest = Request;

                var result = await _businessStatusTables.UpdateStatusTableAsync(_businessStatusTables.UpdateRequest);
                return result == null ?
                    CreateResponse<DTOStatusTables>(result!, StatusCodes.Status200OK, "StatusTable updated successfully.")
                    : 
                    CreateResponse<DTOStatusTables>(null!, StatusCodes.Status500InternalServerError, "Failed to update StatusTable.");

            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("{ID}", Name = "DeleteStatusTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> DeleteAsync([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status200OK, "Bad Ruquest");
                }

                var result = await _businessStatusTables.DeleteStatusTableAsync(ID);

                if (result)
                {
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status200OK, "StatusTable deleted successfully.");
                }
                else
                {
                    return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status500InternalServerError, "StatusTable not found or failed to delete.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}
