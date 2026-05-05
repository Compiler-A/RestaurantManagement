using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.OrderDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ContractsLayerRestaurant.DTOResponse;
using BusinessLayerRestaurant.Mapper;


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
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetailResponse>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if(page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllAsync(page);
            var listResponse = orders.Select(o => o.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOOrderDetailResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");
        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("all-orderid/{orderID}", Name = "GetAllOrderDetailsByOrderID")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetailResponse>>>> GetAllByOrderIDAsync(int orderID)
        {
            if(orderID <= 0)
            {
                throw new ArgumentOutOfRangeException("Order ID must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllByOrderIDAsync(orderID);
            var listResponse = orders.Select(o => o.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOOrderDetailResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetOrderDetailByID")]
        public async Task<ActionResult<ApiResponse<DTOOrderDetailResponse>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var order = await _businessOrderDetail.GetAsync(ID);
            return CreateResponse<DTOOrderDetailResponse>(order!.ToResponse(), StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddNewOrderDetail")]
        public async Task<ActionResult<ApiResponse<DTOOrderDetailResponse>>> CreateAsync
            ([FromBody] DTOOrderDetailsCRequest dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var order = await _businessOrderDetail.IOrder.GetAsync(dto.OrderID);
            var authResult = await authorizationService.AuthorizeAsync(User, order!.EmployeeID, "WaiterOwnerOrAdmin");

            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");


            var result = await _businessOrderDetail.CreateAsync(dto);
            return CreatedAtRoute("GetOrderDetailByID", new { ID = result!.ID }, result.ToResponse());

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateOrderDetail")]
        public async Task<ActionResult<ApiResponse<DTOOrderDetailResponse>>> UpdateAsync
            ([FromBody] DTOOrderDetailsURequest dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var order = await _businessOrderDetail.IOrder.GetAsync(dto.OrderID);
            var authResult = await authorizationService.AuthorizeAsync(User, order!.EmployeeID, "WaiterOwnerOrAdmin");
            if (!authResult.Succeeded)
                throw new UnauthorizedAccessException("Access denied.");


            var result = await _businessOrderDetail.UpdateAsync(dto);
            return CreateResponse<DTOOrderDetailResponse>(result!.ToResponse(), StatusCodes.Status200OK, "Order Detail Updated Successfully!");

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
            var result = await _businessOrderDetail.DeleteAsync(ID);
            return CreateResponse(result, StatusCodes.Status200OK, "Order Detail Deleted Successfully!");
        }
    }
}
