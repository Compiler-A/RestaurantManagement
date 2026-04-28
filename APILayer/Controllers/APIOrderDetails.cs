using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.OrderDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;


namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/OrderDetails")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIOrderDetails : BaseController
    {
        public APIOrderDetails(IOrderDetailsService b)
        {
            _businessOrderDetail = b;
        }

        private readonly IOrderDetailsService _businessOrderDetail;


        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet(Name = "GetAllOrderDetails")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDetail>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if(page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllOrderDetailsAsync(page);
            return CreateResponse<IEnumerable<OrderDetail>>(orders, StatusCodes.Status200OK, $"Row: {orders.Count}");
        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("all-orderid/{orderID}", Name = "GetAllOrderDetailsByOrderID")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDetail>>>> GetAllByOrderIDAsync(int orderID)
        {
            if(orderID <= 0)
            {
                throw new ArgumentOutOfRangeException("Order ID must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllOrderDetailsByOrderIDAsync(orderID);

            return CreateResponse<IEnumerable<OrderDetail>>(orders, StatusCodes.Status200OK, $"Row: {orders.Count}");

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetOrderDetailByID")]
        public async Task<ActionResult<ApiResponse<OrderDetail?>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var order = await _businessOrderDetail.GetOrderDetailAsync(ID);
            return CreateResponse<OrderDetail?>(order, StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = "Manager,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddNewOrderDetail")]
        public async Task<ActionResult<ApiResponse<OrderDetail>>> CreateAsync
            ([FromBody] DTOOrderDetailsCRequest dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var order = await _businessOrderDetail.IOrder.GetOrderAsync(dto.OrderID);
            var authResult = await authorizationService.AuthorizeAsync(User, order!.EmployerID, "WaiterOwnerOrAdmin");

            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");


            var result = await _businessOrderDetail.AddOrderDetailAsync(dto);
            return CreatedAtRoute("GetOrderDetailByID", new { ID = result!.ID }, result);

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateOrderDetail")]
        public async Task<ActionResult<ApiResponse<OrderDetail>>> UpdateAsync
            ([FromBody] DTOOrderDetailsURequest dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var order = await _businessOrderDetail.IOrder.GetOrderAsync(dto.OrderID);
            var authResult = await authorizationService.AuthorizeAsync(User, order!.EmployerID, "WaiterOwnerOrAdmin");
            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");


            var result = await _businessOrderDetail.UpdateOrderDetailAsync(dto);
            return CreateResponse<OrderDetail>(result!, StatusCodes.Status200OK, "Order Detail Updated Successfully!");

        }

        [Authorize(Roles = "Manager,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var result = await _businessOrderDetail.DeleteOrderDetailAsync(ID);
            return CreateResponse(true, StatusCodes.Status200OK, "Order Detail Deleted Successfully!");
        }
    }
}
