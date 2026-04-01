using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant;
using DataLayerRestaurant;

namespace APILayer.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class APIOrders : BaseController
    {
        public APIOrders(IBusinessOrders b)
        {
            _businessOrders = b;
        }

        private readonly IBusinessOrders _businessOrders;

        
       
        [HttpGet(Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrders>>>> GetAllAsync([FromQuery] int page = 1)
        {
            try
            {

                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOOrders>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");
                }
                var orders = await _businessOrders.GetAllOrdersAsync(page);
                if (orders.Count == 0 || orders == null)
                {
                    return CreateResponse<IEnumerable<DTOOrders>>(null!, StatusCodes.Status404NotFound, "not found");
                }
                return CreateResponse<IEnumerable<DTOOrders>>(orders, StatusCodes.Status200OK, $"Count: {orders.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOOrders>>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{ID}", Name ="GetOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders?>>> GetByIDAsync([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOOrders?>(null, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var order = await _businessOrders.GetOrderAsync(ID);
                if (order == null)
                    return CreateResponse<DTOOrders?>(null, StatusCodes.Status404NotFound, "Order not found");

                return CreateResponse<DTOOrders?>(order, StatusCodes.Status200OK, "Find Saccessfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrders?>(null, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("filter", Name = "GetFilterOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOOrders>?>>> GetFilterAsync
            ([FromQuery] DTOOrderFilterRequest Request)
        {
            try
            {
                if (Request.Page <= 0)
                {
                    return CreateResponse<List<DTOOrders>?>(null, StatusCodes.Status400BadRequest, "Page <= 0.");
                }
                var order = await _businessOrders.GetFilterOrdersAsync(Request);
                if (order == null || order.Count == 0)
                    return CreateResponse<List<DTOOrders>?>(null, StatusCodes.Status404NotFound, "Order not found");

                return CreateResponse<List<DTOOrders>?>(order, StatusCodes.Status200OK, "Find Saccessfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<List<DTOOrders>?>(null, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost(Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> CreateAsync([FromBody] DTOOrderCRequest dto)
        {
            try
            {
                if (dto == null || dto.StatusOrderID <= 0 || dto.TableID <= 0 || dto.EmployerID <= 0)
                {
                    return CreateResponse<DTOOrders>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest.");
                }
                _businessOrders.CreateRequest = dto;
                var result = await _businessOrders.AddOrderAsync(_businessOrders.CreateRequest);
                if (result == null)
                    return CreateResponse<DTOOrders>(null!, StatusCodes.Status500InternalServerError, "Failed to create order");

                return CreatedAtRoute("GetOrderByID", new {ID = result.ID}, result);

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrders>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> UpdateAsync([FromBody] DTOOrderURequest dto)
        {
            try
            {
                if (dto == null || dto.StatusOrderID <= 0 || dto.TableID <= 0 || dto.EmployerID <= 0 )
                {
                    return CreateResponse<DTOOrders>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest.");
                }

                var DTO = await _businessOrders.GetOrderAsync(dto.OrderID);
                if (DTO == null)
                {
                    return CreateResponse<DTOOrders>(null!, StatusCodes.Status404NotFound, "Failed, Not Found");
                }
                _businessOrders.UpdateRequest = dto;
                var result = await _businessOrders.UpdateOrderAsync(_businessOrders.UpdateRequest);
                if (result == null)
                    return CreateResponse<DTOOrders>(null!, StatusCodes.Status500InternalServerError, "Failed to update order");

                return CreateResponse<DTOOrders>(result, StatusCodes.Status200OK, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrders>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<bool>(false, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var result = await _businessOrders.DeleteOrderAsync(ID);
                if (!result)
                    return CreateResponse(false, StatusCodes.Status404NotFound, "Failed to delete order");

                return CreateResponse(true, StatusCodes.Status200OK, "Order deleted successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse(false, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
