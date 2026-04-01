using BusinessLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayerRestaurant;

namespace APILayer.Controllers
{
    [Route("api/OrderDetails")]
    [ApiController]
    public class APIOrderDetails : BaseController
    {
        public APIOrderDetails(IBusinessOrderDetails b)
        {
            _businessOrderDetail = b;
        }

        private readonly IBusinessOrderDetails _businessOrderDetail;

        

        [HttpGet(Name = "GetAllOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetails>>>> GetAllAsync([FromQuery] int page = 1)
        {
            try
            {

                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");
                }
                var orders = await _businessOrderDetail.GetAllOrderDetailsAsync(page);
                if (orders.Count == 0 || orders == null)
                {
                    return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, StatusCodes.Status404NotFound, "not found");
                }
                return CreateResponse<IEnumerable<DTOOrderDetails>>(orders, StatusCodes.Status200OK, $"Count: {orders.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("all-orderid/{orderID}", Name = "GetAllOrderDetailsByOrderID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetails>>>> GetAllByOrderIDAsync(int orderID)
        {
            try
            {
                if (orderID <= 0)
                {
                    return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, StatusCodes.Status400BadRequest, "Order ID must be greater than 0.");
                }
                var orders = await _businessOrderDetail.GetAllOrderDetailsByOrderIDAsync(orderID);
                if (orders.Count == 0 || orders == null)
                {
                    return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, StatusCodes.Status404NotFound, "not found");
                }
                return CreateResponse<IEnumerable<DTOOrderDetails>>(orders, StatusCodes.Status200OK, $"Count: {orders.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{ID}", Name = "GetOrderDetailByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails?>>> GetByIDAsync(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOOrderDetails?>(null, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var order = await _businessOrderDetail.GetOrderDetailAsync(ID);
                if (order == null)
                    return CreateResponse<DTOOrderDetails?>(null, StatusCodes.Status404NotFound, "Order detail not found");

                return CreateResponse<DTOOrderDetails?>(order, StatusCodes.Status200OK, "Find Saccessfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrderDetails?>(null, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost(Name = "AddNewOrderDetail")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> CreateAsync([FromBody] DTOOrderDetailsCRequest dto)
        {
            try
            {
                if (dto == null || dto.ItemID <= 0 || dto.OrderID <= 0 || dto.Quantity <= 0)
                {
                    return CreateResponse<DTOOrderDetails>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest.");
                }
                _businessOrderDetail.CreateRequest = dto;
                var result = await _businessOrderDetail.AddOrderDetailAsync(_businessOrderDetail.CreateRequest);
                if (result == null)
                    return CreateResponse<DTOOrderDetails>(null!, StatusCodes.Status500InternalServerError, "Failed to create order detail");

                return CreatedAtRoute("GetOrderDetailByID", new {ID = result.ID}, result);

            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrderDetails>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut(Name = "UpdateOrderDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> UpdateAsync([FromBody] DTOOrderDetailsURequest dto)
        {
            try
            {
                if (dto == null || dto.OrderID <= 0 || dto.ItemID <= 0 || dto.Quantity <= 0)
                {
                    return CreateResponse<DTOOrderDetails>(null!, StatusCodes.Status400BadRequest, "Bad Ruquest.");
                }

                 ;
                if(await _businessOrderDetail.GetOrderDetailAsync(dto.ID) == null)
                {
                    return CreateResponse<DTOOrderDetails>(null!, StatusCodes.Status404NotFound, "Failed ,to find Order Detail");
                }
                _businessOrderDetail.UpdateRequest = dto;
                var result = await _businessOrderDetail.UpdateOrderDetailAsync(_businessOrderDetail.UpdateRequest);
                if (result == null)
                    return CreateResponse<DTOOrderDetails>(null!, StatusCodes.Status400BadRequest, "Failed to update order");

                return CreateResponse<DTOOrderDetails>(result, StatusCodes.Status200OK, "Order Detail updated successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrderDetails>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
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
                var result = await _businessOrderDetail.DeleteOrderDetailAsync(ID);
                if (!result)
                    return CreateResponse(false, StatusCodes.Status400BadRequest, "Failed to delete order");

                return CreateResponse(true, StatusCodes.Status200OK, "Order detail deleted successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse(false, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
