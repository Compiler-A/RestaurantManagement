using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;
using ContractsLayerRestaurant.DTOResponse;
using BusinessLayerRestaurant.Mapper;


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
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderResponse>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var orders = await _businessOrders.GetAllAsync(page);
            var listReponse = orders.Select(o => o.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOOrderResponse>>(listReponse, StatusCodes.Status200OK, $"Row: {listReponse.Count}");

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name ="GetOrderByID")]
        public async Task<ActionResult<ApiResponse<DTOOrderResponse>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var order = await _businessOrders.GetAsync(ID);
            return CreateResponse<DTOOrderResponse>(order!.ToResponse(), StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [HttpGet("filter", Name = "GetFilterOrder")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOOrderResponse>>>> GetFilterAsync
            ([FromQuery] DTOOrderFilterRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var order = await _businessOrders.GetFilterAsync(Request);
            var listResponse = order!.Select(o => o.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOOrderResponse>>(listResponse, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddNewOrder")]
        public async Task<ActionResult<ApiResponse<DTOOrderResponse>>> CreateAsync
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
            return CreatedAtRoute("GetOrderByID", new { ID = result!.ID }, result.ToResponse());

        }

        [Authorize(Roles = "Manager,Chef,Sous Chef,Waiter")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateOrder")]
        public async Task<ActionResult<ApiResponse<DTOOrderResponse>>> UpdateAsync
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
            return CreateResponse<DTOOrderResponse>(result!.ToResponse(), StatusCodes.Status200OK, "Order Updated Successfully!");

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
            return CreateResponse(result, StatusCodes.Status200OK, "Order Deleted Successfully!");

        }
    }
}
