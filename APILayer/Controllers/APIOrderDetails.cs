using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.OrderDetails;
using Microsoft.AspNetCore.Authorization;


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
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetails>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if(page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllOrderDetailsAsync(page);
            return CreateResponse<IEnumerable<DTOOrderDetails>>(orders, StatusCodes.Status200OK, $"Row: {orders.Count}");
        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("all-orderid/{orderID}", Name = "GetAllOrderDetailsByOrderID")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderDetails>>>> GetAllByOrderIDAsync(int orderID)
        {
            if(orderID <= 0)
            {
                throw new ArgumentOutOfRangeException("Order ID must be greater than 0.");
            }
            var orders = await _businessOrderDetail.GetAllOrderDetailsByOrderIDAsync(orderID);

            return CreateResponse<IEnumerable<DTOOrderDetails>>(orders, StatusCodes.Status200OK, $"Row: {orders.Count}");

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("{ID}", Name = "GetOrderDetailByID")]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails?>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var order = await _businessOrderDetail.GetOrderDetailAsync(ID);
            return CreateResponse<DTOOrderDetails?>(order, StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = "Manager,Waiter")]
        [HttpPost(Name = "AddNewOrderDetail")]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> CreateAsync
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
        [HttpPut(Name = "UpdateOrderDetail")]
        public async Task<ActionResult<ApiResponse<DTOOrderDetails>>> UpdateAsync
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
            return CreateResponse<DTOOrderDetails>(result!, StatusCodes.Status200OK, "Order Detail Updated Successfully!");

        }

        [Authorize(Roles = "Manager,Waiter")]
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
