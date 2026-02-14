using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;

namespace APILayer.Controllers
{
    [Route("api/APIStatusTables")]
    [ApiController]
    public class APIStatusTables : BaseController
    {

        private readonly IBusinessStatusTables _businessStatusTables;

        public APIStatusTables(IBusinessStatusTables businessStatusTables)
        {
            _businessStatusTables = businessStatusTables;
        }

        [HttpGet("GetAllStatusTables/{Page}", Name = "GetAllStatusTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOStatusTables>>>> GetAllStatusTables([FromRoute] int Page)
        {
            try
            {
                if (Page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOStatusTables>>(null!, 400, "Page number must be greater than zero.");
                }

                var list = await _businessStatusTables.GetAll(Page);
                if (list == null || list.Count == 0)
                    return CreateResponse<IEnumerable<DTOStatusTables>>(null!, 404, "No StatusTables found.");

                return CreateResponse<IEnumerable<DTOStatusTables>>(list!, 200, "Completed!");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<IEnumerable<DTOStatusTables>>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetStatusTable/{ID}", Name = "GetStatusTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> GetStatusTable([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOStatusTables>(null!, 400, "Bad Ruquest");
                }
                var statusTable = await _businessStatusTables.LoadByID(ID);
                if (statusTable == null)
                    return CreateResponse<DTOStatusTables>(null!, 404, "StatusTable not found.");

                
                return CreateResponse<DTOStatusTables>(statusTable!, 200, "Completed!");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, 500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost("AddStatusTable", Name = "AddStatusTable")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> AddStatusTable([FromBody] DTOStatusTables newStatusTable)
        {
            try
            {
                if (newStatusTable == null)
                {

                    return CreateResponse<DTOStatusTables>(null!, 400, "Bad Ruquest");
                }

                var business = new clsBusinessStatusTables(new clsDataStatusTables() ,newStatusTable, clsBusinessStatusTables.enMode.Add);

                var result = await business.Save();
                if (result)
                { 
                    return CreatedAtRoute("GetStatusTable", new { ID = business.StatusTable!.StatusTableID }, newStatusTable );
                }
                else
                {
                    return CreateResponse<DTOStatusTables>(null!, 400, "Failed to add StatusTable.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        

        [HttpPut("UpdateStatusTable", Name = "UpdateStatusTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> UpdateStatusTable([FromBody] DTOStatusTables updatedStatusTable)
        {
            try
            {
                if (updatedStatusTable == null || updatedStatusTable.StatusTableID <= 0)
                {
                    return CreateResponse<DTOStatusTables>(null!, 400, "Bad Ruquest");
                }

                var business = new clsBusinessStatusTables();
                var existingStatusTable = await business.LoadByID(updatedStatusTable.StatusTableID);

                if (existingStatusTable == null)
                {
                    return CreateResponse<DTOStatusTables>(null!, 404, "StatusTable not found.");
                }

                business.StatusTable = updatedStatusTable;

                var result = await business.Save();
                return result ?
                    CreateResponse<DTOStatusTables>(business.StatusTable, 200, "StatusTable updated successfully.")
                    : 
                    CreateResponse<DTOStatusTables>(null!, 500, "Failed to update StatusTable.");

            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, 500, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("DeleteStatusTable/{id}", Name = "DeleteStatusTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusTables>>> DeleteStatusTable([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<DTOStatusTables>(null!, 400, "Bad Ruquest");
                }

                var result = await _businessStatusTables.Delete(id);

                if (result)
                {
                    return CreateResponse<DTOStatusTables>(null!, 200, "StatusTable deleted successfully.");
                }
                else
                {
                    return CreateResponse<DTOStatusTables>(null!, 404, "StatusTable not found or failed to delete.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusTables>(null!, 500, "Internal server error: " + ex.Message);
            }
        }
    }
}
