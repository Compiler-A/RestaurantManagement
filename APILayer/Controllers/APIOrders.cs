using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Orders;


namespace APILayer.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIOrders : BaseController
    {
        public APIOrders(IOrdersService b)
        {
            _businessOrders = b;
        }

        private readonly IOrdersService _businessOrders;

        
       
        [HttpGet(Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrders>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrders.GetAllOrdersAsync(page);
            return CreateResponse<IEnumerable<DTOOrders>>(orders, StatusCodes.Status200OK, $"Count: {orders.Count}");

        }

        [HttpGet("{ID}", Name ="GetOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders?>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var order = await _businessOrders.GetOrderAsync(ID);
            return CreateResponse<DTOOrders?>(order, StatusCodes.Status200OK, "Find Saccessfully");
        }


        [HttpGet("filter", Name = "GetFilterOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOOrders>?>>> GetFilterAsync
            ([FromQuery] DTOOrderFilterRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var order = await _businessOrders.GetFilterOrdersAsync(Request);
            return CreateResponse<List<DTOOrders>?>(order, StatusCodes.Status200OK, "Find Saccessfully");
        }

        [HttpPost(Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> CreateAsync([FromBody] DTOOrderCRequest dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await _businessOrders.AddOrderAsync(dto);
            return CreatedAtRoute("GetOrderByID", new { ID = result!.ID }, result);

        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> UpdateAsync([FromBody] DTOOrderURequest dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _businessOrders.UpdateOrderAsync(dto);
            return CreateResponse<DTOOrders>(result!, StatusCodes.Status200OK, "Order updated successfully");

        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await _businessOrders.DeleteOrderAsync(ID);
            return CreateResponse(true, StatusCodes.Status200OK, "Order deleted successfully");

        }
    }
}
