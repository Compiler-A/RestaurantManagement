using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/APIOrders")]
    [ApiController]
    public class APIOrders : BaseController
    {
        public APIOrders(IBusinessOrders b)
        {
            _businessOrders = b;
        }

        private readonly IBusinessOrders _businessOrders;

        
       
        [HttpGet("GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrders>>>> GetAll([FromQuery] int page = 1)
        {
            try
            {

                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOOrders>>(null!, 400, "Page number must be greater than 0.");
                }
                var orders = await _businessOrders.GetAllOrdersAsync(page);
                if (orders.Count == 0 || orders == null)
                {
                    return CreateResponse<IEnumerable<DTOOrders>>(null!, 404, "not found");
                }
                return CreateResponse<IEnumerable<DTOOrders>>(orders, 200, $"Count: {orders.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOOrders>>(null!, 500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetOrder/{id}", Name ="GetOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders?>>> GetOrderByID([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<DTOOrders?>(null, 400, "ID <= 0.");
                }
                var order = await _businessOrders.GetOrderAsync(id);
                if (order == null)
                    return CreateResponse<DTOOrders?>(null, 404, "Order not found");

                return CreateResponse<DTOOrders?>(order, 200, "Find Saccessfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrders?>(null, 500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetFilterOrder", Name = "GetFilterOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOOrders>?>>> GetFilterOrder
            ([FromQuery] int Page, [FromQuery] int TableID, [FromQuery] int EmployeeID, [FromQuery] int StatusOrderID)
        {
            try
            {
                if (Page <= 0)
                {
                    return CreateResponse<List<DTOOrders>?>(null, 400, "Page <= 0.");
                }
                var order = await _businessOrders.GetFilterOrdersAsync(Page, TableID,EmployeeID, StatusOrderID);
                if (order == null || order.Count == 0)
                    return CreateResponse<List<DTOOrders>?>(null, 404, "Order not found");

                return CreateResponse<List<DTOOrders>?>(order, 200, "Find Saccessfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<List<DTOOrders>?>(null, 500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddNewOrder", Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> AddNewOrder([FromBody] DTOOrderCreateRequest dto)
        {
            try
            {
                if (dto == null || dto.StatusOrderID <= 0 || dto.TableID <= 0 || dto.EmployerID <= 0)
                {
                    return CreateResponse<DTOOrders>(null!, 400, "Bad Ruquest.");
                }
                _businessOrders.DTOOrderRequest = dto;
                var result = await _businessOrders.SaveAsync();
                if (!result)
                    return CreateResponse<DTOOrders>(null!, 400, "Failed to create order");

                return CreateResponse<DTOOrders>(_businessOrders.DTOOrders, 200, "Order Added successfully");

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrders>(null!, 500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/orders/5
        [HttpPut("UpdateOrder" , Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> UpdateOrder([FromBody] DTOOrderUpdateRequest dto)
        {
            try
            {
                if (dto == null || dto.StatusOrderID <= 0 || dto.TableID <= 0 || dto.EmployerID <= 0 )
                {
                    return CreateResponse<DTOOrders>(null!, 400, "Bad Ruquest.");
                }

                _businessOrders.DTOOrders = await _businessOrders.GetOrderAsync(dto.OrderID);
                if (_businessOrders.DTOOrders == null)
                {
                    return CreateResponse<DTOOrders>(null!, 400, "Failed, Not Found");
                }
                _businessOrders.DTOOrderUpdateRequest = dto;
                var result = await _businessOrders.SaveAsync();
                if (!result)
                    return CreateResponse<DTOOrders>(null!, 400, "Failed to update order");

                return CreateResponse<DTOOrders>(_businessOrders.DTOOrders, 200, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrders>(null!, 500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/orders/5
        [HttpDelete("DeleteOrder/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<bool>(false, 400, "ID <= 0.");
                }
                var result = await _businessOrders.DeleteAsync(id);
                if (!result)
                    return CreateResponse(false, 400, "Failed to delete order");

                return CreateResponse(true, 200, "Order deleted successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse(false, 500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
