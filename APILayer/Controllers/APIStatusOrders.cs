using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace APILayer.Controllers
{
    [Route("api/StatusOrders")]
    [ApiController]
    public class APIStatusOrders : BaseController
    {

        private readonly IBusinessStatusOrders _StatusOrder;
        public APIStatusOrders(IBusinessStatusOrders statusOrder)
        {
            _StatusOrder = statusOrder;
        }

        [HttpGet( Name = "GetAllStatusOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOStatusOrders>>>> GetAllAsync([FromQuery] int page = 1)
        {
            try
            {
                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOStatusOrders>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");
                }
                var list = await _StatusOrder.GetAllStatusOrdersAsync(page);

                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOStatusOrders>>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }

                return CreateResponse<IEnumerable<DTOStatusOrders>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOStatusOrders>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{ID}", Name = "GetStatusOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status200OK, "ID <= 0.");
                }
                var DTO = await _StatusOrder.GetStatusOrdersAsync(ID);

                if (DTO == null)
                {
                    return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }

                return CreateResponse<DTOStatusOrders>(DTO, StatusCodes.Status200OK, "Find Saccessfully!");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost(Name = "AddStatusOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> CreateAsync([FromBody] DTOStatusOrdersCRequest Request)
        {
            try
            {
                if (Request == null)
                    return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status400BadRequest, "Body is null.");

                if (string.IsNullOrWhiteSpace(Request.Name))
                    return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status400BadRequest, "Name is required.");

                _StatusOrder.CreateRequest = Request;

                var result = await _StatusOrder.AddStatusOrdersAsync(_StatusOrder.CreateRequest);

                if (result == null)
                    return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status500InternalServerError, "Insert failed.");

                return CreatedAtRoute("GetStatusOrderByID", new {ID = result.ID}, result);

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut(Name = "UpdateStatusOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> UpdateAsync([FromBody] DTOStatusOrdersURequest Request)
        {
            try
            {
                if (Request == null || Request.ID <= 0)
                {
                    return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }
                
                var existingStatusOrder = await _StatusOrder.GetStatusOrdersAsync(Request.ID);

                if (existingStatusOrder == null)
                {
                    return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status404NotFound, "StatusTable not found.");
                }   

                _StatusOrder.UpdateRequest = Request;
                var result = await _StatusOrder.UpdateStatusOrdersAsync(_StatusOrder.UpdateRequest);
                return result != null ?
                    CreateResponse<DTOStatusOrders>(result, StatusCodes.Status200OK, "StatusOrder updated successfully.")
                    :
                    CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status500InternalServerError, "Failed to update StatusOrders.");

            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusOrders>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("{ID}",Name = "DeleteStatusOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<bool>(false!, StatusCodes.Status400BadRequest, "Bad Ruquest");
                }

                var result = await _StatusOrder.DeleteStatusOrdersAsync(ID);

                if (result)
                {
                    return CreateResponse<bool>(true!, StatusCodes.Status200OK, "StatusOrder deleted successfully.");
                }
                else
                {
                    return CreateResponse<bool>(false!, StatusCodes.Status404NotFound, "StatusTable not found or failed to delete.");
                }
            }
            catch (System.Exception ex)
            {
                return CreateResponse<bool>(false!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}
