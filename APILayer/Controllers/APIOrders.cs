using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;


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
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Order>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrders.GetAllAsync(page);
            return CreateResponse<IEnumerable<Order>>(orders, StatusCodes.Status200OK, $"Row: {orders.Count}");

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name ="GetOrderByID")]
        public async Task<ActionResult<ApiResponse<Order?>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var order = await _businessOrders.GetAsync(ID);
            return CreateResponse<Order?>(order, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("filter", Name = "GetFilterOrder")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<List<Order>?>>> GetFilterAsync
            ([FromQuery] DTOOrderFilterRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var order = await _businessOrders.GetFilterAsync(Request);
            return CreateResponse<List<Order>?>(order, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddNewOrder")]
        public async Task<ActionResult<ApiResponse<Order>>> CreateAsync
            ([FromBody] DTOOrderCRequest dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var authResult = await authorizationService.AuthorizeAsync(User, dto.EmployeeID, "WaiterOwnerOrAdmin");

            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");
            var result = await _businessOrders.CreateAsync(dto);
            return CreatedAtRoute("GetOrderByID", new { ID = result!.ID }, result);

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateOrder")]
        public async Task<ActionResult<ApiResponse<Order>>> UpdateAsync
            ([FromBody] DTOOrderURequest dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var authResult = await authorizationService.AuthorizeAsync(User, dto.EmployeeID, "WaiterOwnerOrAdmin");

            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");

            var result = await _businessOrders.UpdateAsync(dto);
            return CreateResponse<Order>(result!, StatusCodes.Status200OK, "Order Updated Successfully!");

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            var result = await _businessOrders.DeleteAsync(ID);
            return CreateResponse(true, StatusCodes.Status200OK, "Order Deleted Successfully!");

        }
    }
}
