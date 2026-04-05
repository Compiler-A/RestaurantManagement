using BusinessLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayerRestaurant;

namespace APILayer.Controllers
{
    [Route("api/OrderDetails")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
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
            if(page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllOrderDetailsAsync(page);
            return CreateResponse<IEnumerable<DTOOrderDetails>>(orders, StatusCodes.Status200OK, $"Count: {orders.Count}");
        }


        [HttpGet("all-orderid/{orderID}", Name = "GetAllOrderDetailsByOrderID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetails>>>> GetAllByOrderIDAsync(int orderID)
        {
            if(orderID <= 0)
            {
                throw new ArgumentOutOfRangeException("Order ID must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllOrderDetailsByOrderIDAsync(orderID);

            return CreateResponse<IEnumerable<DTOOrderDetails>>(orders, StatusCodes.Status200OK, $"Count: {orders.Count}");

        }

        [HttpGet("{ID}", Name = "GetOrderDetailByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails?>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var order = await _businessOrderDetail.GetOrderDetailAsync(ID);
            return CreateResponse<DTOOrderDetails?>(order, StatusCodes.Status200OK, "Find Saccessfully");

        }

        [HttpPost(Name = "AddNewOrderDetail")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> CreateAsync([FromBody] DTOOrderDetailsCRequest dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _businessOrderDetail.AddOrderDetailAsync(dto);
            return CreatedAtRoute("GetOrderDetailByID", new { ID = result!.ID }, result);

        }

        [HttpPut(Name = "UpdateOrderDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> UpdateAsync([FromBody] DTOOrderDetailsURequest dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _businessOrderDetail.UpdateOrderDetailAsync(dto);
            return CreateResponse<DTOOrderDetails>(result!, StatusCodes.Status200OK, "Order Detail updated successfully");

        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await _businessOrderDetail.DeleteOrderDetailAsync(ID);
            return CreateResponse(true, StatusCodes.Status200OK, "Order detail deleted successfully");
        }
    }
}
