using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;


namespace APILayer.Controllers
{
    [Authorize]
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


        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet(Name = "GetAllOrders")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrders>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrders.GetAllOrdersAsync(page);
            return CreateResponse<IEnumerable<DTOOrders>>(orders, StatusCodes.Status200OK, $"Row: {orders.Count}");

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("{ID}", Name ="GetOrderByID")]
        public async Task<ActionResult<ApiResponse<DTOOrders?>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var order = await _businessOrders.GetOrderAsync(ID);
            return CreateResponse<DTOOrders?>(order, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("filter", Name = "GetFilterOrder")]
        public async Task<ActionResult<ApiResponse<List<DTOOrders>?>>> GetFilterAsync
            ([FromQuery] DTOOrderFilterRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var order = await _businessOrders.GetFilterOrdersAsync(Request);
            return CreateResponse<List<DTOOrders>?>(order, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Waiter")]
        [HttpPost(Name = "AddNewOrder")]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> CreateAsync([FromBody] DTOOrderCRequest dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await _businessOrders.AddOrderAsync(dto);
            return CreatedAtRoute("GetOrderByID", new { ID = result!.ID }, result);

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpPut(Name = "UpdateOrder")]
        public async Task<ActionResult<ApiResponse<DTOOrders>>> UpdateAsync([FromBody] DTOOrderURequest dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _businessOrders.UpdateOrderAsync(dto);
            return CreateResponse<DTOOrders>(result!, StatusCodes.Status200OK, "Order Updated Successfully!");

        }

        [Authorize(Roles = "Manager,Waiter")]
        [HttpDelete("{ID}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await _businessOrders.DeleteOrderAsync(ID);
            return CreateResponse(true, StatusCodes.Status200OK, "Order Deleted Successfully!");

        }
    }
}
