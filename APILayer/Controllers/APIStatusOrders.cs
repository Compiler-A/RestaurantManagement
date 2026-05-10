using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ContractsLayerRestaurant.DTOResponse;
using BusinessLayerRestaurant.Mapper;


namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/StatusOrders")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIStatusOrders : BaseController
    {

        private readonly IStatusOrdersService _StatusOrder;
        public APIStatusOrders(IStatusOrdersService statusOrder)
        {
            _StatusOrder = statusOrder;
        }

        [AllowAnonymous]
        [HttpGet( Name = "GetAllStatusOrders")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOStatusOrderResponse>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var list = await _StatusOrder.GetAllAsync(page);
            var listResponse = list.Select(x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOStatusOrderResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetStatusOrderByID")]
        public async Task<ActionResult<ApiResponse<DTOStatusOrderResponse>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var DTO = await _StatusOrder.GetAsync(ID);
            return CreateResponse<DTOStatusOrderResponse>(DTO!.ToResponse(), StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddStatusOrder")]
        public async Task<ActionResult<ApiResponse<DTOStatusOrderResponse>>> CreateAsync([FromBody] DTOStatusOrdersCRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await _StatusOrder.CreateAsync(Request);
            return CreatedAtRoute("GetStatusOrderByID", new { ID = result!.ID }, result.ToResponse());

        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateStatusOrder")]
        public async Task<ActionResult<ApiResponse<DTOStatusOrderResponse>>> UpdateAsync([FromBody] DTOStatusOrdersURequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _StatusOrder.UpdateAsync(Request);
            return CreateResponse<DTOStatusOrderResponse>(result!.ToResponse(), StatusCodes.Status200OK, "Status Order Updated Successfully!");
        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}",Name = "DeleteStatusOrder")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var result = await _StatusOrder.DeleteAsync(ID);
            return CreateResponse<bool>(result, StatusCodes.Status200OK, "Status Order Deleted Successfully!");
        }
    }
}
