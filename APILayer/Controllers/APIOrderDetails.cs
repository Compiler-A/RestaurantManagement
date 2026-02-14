using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/APIOrderDetails")]
    [ApiController]
    public class APIOrderDetails : BaseController
    {
        public APIOrderDetails(IBusinessOrderDetails b)
        {
            _businessOrderDetail = b;
        }

        private readonly IBusinessOrderDetails _businessOrderDetail;



        [HttpGet("GetAllOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetails>>>> GetAll([FromQuery] int page = 1)
        {
            try
            {

                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, 400, "Page number must be greater than 0.");
                }
                var orders = await _businessOrderDetail.GetAllOrderDetailsAsync(page);
                if (orders.Count == 0 || orders == null)
                {
                    return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, 404, "not found");
                }
                return CreateResponse<IEnumerable<DTOOrderDetails>>(orders, 200, $"Count: {orders.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOOrderDetails>>(null!, 500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetOrderDetail/{id}", Name = "GetOrderDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails?>>> GetOrderDetailByID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<DTOOrderDetails?>(null, 400, "ID <= 0.");
                }
                var order = await _businessOrderDetail.GetOrderDetailsAsync(id);
                if (order == null)
                    return CreateResponse<DTOOrderDetails?>(null, 404, "Order detail not found");

                return CreateResponse<DTOOrderDetails?>(order, 200, "Find Saccessfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrderDetails?>(null, 500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddNewOrderDetail", Name = "AddNewOrderDetail")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> AddNewOrderDetail([FromBody] DTOOrderDetails dto)
        {
            try
            {
                if (dto == null || dto.ItemID <= 0 || dto.OrderID <= 0 || dto.Quantity <= 0)
                {
                    return CreateResponse<DTOOrderDetails>(null!, 400, "Bad Ruquest.");
                }
                _businessOrderDetail.dtoOrderDetail = dto;
                var result = await _businessOrderDetail.SaveAsync();
                if (!result)
                    return CreateResponse<DTOOrderDetails>(null!, 400, "Failed to create order detail");

                return CreatedAtRoute(
                    "GetOrderDetail",
                    new { ID = this._businessOrderDetail.dtoOrderDetail!.ID },
                    _businessOrderDetail.dtoOrderDetail
                );
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrderDetails>(null!, 500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/orders/5
        [HttpPut("UpdateOrderDetail", Name = "UpdateOrderDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> UpdateOrderDetail([FromBody] DTOOrderDetails dto)
        {
            try
            {
                if (dto == null || dto.OrderID <= 0 || dto.ItemID <= 0 || dto.Quantity <= 0)
                {
                    return CreateResponse<DTOOrderDetails>(null!, 400, "Bad Ruquest.");
                }

                _businessOrderDetail.dtoOrderDetail = await _businessOrderDetail.GetOrderDetailsAsync(dto.ID);
                if(_businessOrderDetail.dtoOrderDetail == null)
                {
                    return CreateResponse<DTOOrderDetails>(null!, 400, "Failed ,to find Order Detail");
                }
                _businessOrderDetail.dtoOrderDetail = dto;
                var result = await _businessOrderDetail.SaveAsync();
                if (!result)
                    return CreateResponse<DTOOrderDetails>(null!, 400, "Failed to update order");

                return CreateResponse<DTOOrderDetails>(_businessOrderDetail.dtoOrderDetail, 200, "Order Detail updated successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOOrderDetails>(null!, 500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/orders/5
        [HttpDelete("DeleteOrderDetail/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOrderDetail(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return CreateResponse<bool>(false, 400, "ID <= 0.");
                }
                var result = await _businessOrderDetail.DeleteAsync(id);
                if (!result)
                    return CreateResponse(false, 400, "Failed to delete order");

                return CreateResponse(true, 200, "Order detail deleted successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse(false, 500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
