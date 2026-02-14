using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/APIStatusOrders")]
    [ApiController]
    public class APIStatusOrders : BaseController
    {

        private readonly IBusinessStatusOrder _StatusOrder;
        public APIStatusOrders(IBusinessStatusOrder statusOrder)
        {
            _StatusOrder = statusOrder;
        }

        [HttpGet("GetAllStatusOrders", Name = "GetAllStatusOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOStatusOrders>>>> GetAllStatusOrders([FromQuery] int page = 1)
        {
            try
            {
                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOStatusOrders>>(null!, 400, "Page number must be greater than 0.");
                }
                var list = await _StatusOrder.GetAllStatusOrders(page);

                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOStatusOrders>>(null!, 404, "Not Found!");
                }

                return CreateResponse<IEnumerable<DTOStatusOrders>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOStatusOrders>>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetStatusOrder/{ID}", Name = "GetStatusOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> GetStatusOrderByID([FromRoute] int ID = 1)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOStatusOrders>(null!, 400, "ID <= 0.");
                }
                var DTO = await _StatusOrder.GetStatusOrdersByID(ID);

                if (DTO == null)
                {
                    return CreateResponse<DTOStatusOrders>(null!, 404, "Not Found!");
                }

                return CreateResponse<DTOStatusOrders>(DTO, 200, "Find Saccessfully!");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOStatusOrders>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddStatusOrder", Name = "AddStatusOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> AddStatusOrder([FromBody] DTOStatusOrders StatusOrder)
        {
            try
            {
                if (StatusOrder == null)
                    return CreateResponse<DTOStatusOrders>(null!, 400, "Body is null.");

                if (string.IsNullOrWhiteSpace(StatusOrder.statusOrderName))
                    return CreateResponse<DTOStatusOrders>(null!, 400, "Name is required.");

                _StatusOrder.StatusOrders = StatusOrder;

                var result = await _StatusOrder.Save();

                if (!result)
                    return CreateResponse<DTOStatusOrders>(null!, 500, "Insert failed.");

                return CreatedAtRoute(
                    "GetStatusOrder",
                    new { ID = _StatusOrder.StatusOrders!.idStatusOrder },
                    _StatusOrder.StatusOrders
                );
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOStatusOrders>(null!, 500, ex.Message);
            }
        }

        [HttpPut("UpdateStatusOrder", Name = "UpdateStatusOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> UpdateStatusOrder([FromBody] DTOStatusOrders Update)
        {
            try
            {
                if (Update == null || Update.idStatusOrder <= 0)
                {
                    return CreateResponse<DTOStatusOrders>(null!, 400, "Bad Ruquest");
                }
                
                var existingStatusOrder = await _StatusOrder.GetStatusOrdersByID(Update.idStatusOrder);

                if (existingStatusOrder == null)
                {
                    return CreateResponse<DTOStatusOrders>(null!, 404, "StatusTable not found.");
                }   


                var result = await _StatusOrder.Save();
                return result ?
                    CreateResponse<DTOStatusOrders>(_StatusOrder.StatusOrders!, 200, "StatusOrder updated successfully.")
                    :
                    CreateResponse<DTOStatusOrders>(null!, 500, "Failed to update StatusOrders.");

            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusOrders>(null!, 500, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("DeleteStatusOrder/{id}", Name = "DeleteStatusOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> DeleteStatusOrder([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<DTOStatusOrders>(null!, 400, "Bad Ruquest");
                }

                var result = await _StatusOrder.Delete(id);

                if (result)
                {
                    return CreateResponse<DTOStatusOrders>(null!, 200, "StatusOrder deleted successfully.");
                }
                else
                {
                    return CreateResponse<DTOStatusOrders>(null!, 404, "StatusTable not found or failed to delete.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusOrders>(null!, 500, "Internal server error: " + ex.Message);
            }
        }
    }
}
