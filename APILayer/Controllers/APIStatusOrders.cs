using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;


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
        public async Task<ActionResult<ApiResponse<IEnumerable<StatusOrder>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var list = await _StatusOrder.GetAllStatusOrdersAsync(page);
            return CreateResponse<IEnumerable<StatusOrder>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetStatusOrderByID")]
        public async Task<ActionResult<ApiResponse<StatusOrder>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var DTO = await _StatusOrder.GetStatusOrdersAsync(ID);
            return CreateResponse<StatusOrder>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddStatusOrder")]
        public async Task<ActionResult<ApiResponse<StatusOrder>>> CreateAsync([FromBody] DTOStatusOrdersCRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await _StatusOrder.AddStatusOrdersAsync(Request);
            return CreatedAtRoute("GetStatusOrderByID", new { ID = result!.ID }, result);

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateStatusOrder")]
        public async Task<ActionResult<ApiResponse<StatusOrder>>> UpdateAsync([FromBody] DTOStatusOrdersURequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _StatusOrder.UpdateStatusOrdersAsync(Request);
            return CreateResponse<StatusOrder>(result!, StatusCodes.Status200OK, "Status Order Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}",Name = "DeleteStatusOrder")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var result = await _StatusOrder.DeleteStatusOrdersAsync(ID);
            return CreateResponse<bool>(true!, StatusCodes.Status200OK, "Status Order Deleted Successfully!");
        }
    }
}
