using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusOrders;
using Microsoft.AspNetCore.Authorization;


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
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOStatusOrders>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var list = await _StatusOrder.GetAllStatusOrdersAsync(page);
            return CreateResponse<IEnumerable<DTOStatusOrders>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [HttpGet("{ID}", Name = "GetStatusOrderByID")]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }

            var DTO = await _StatusOrder.GetStatusOrdersAsync(ID);
            return CreateResponse<DTOStatusOrders>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(Name = "AddStatusOrder")]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> CreateAsync([FromBody] DTOStatusOrdersCRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var result = await _StatusOrder.AddStatusOrdersAsync(Request);
            return CreatedAtRoute("GetStatusOrderByID", new { ID = result!.ID }, result);

        }

        [Authorize(Roles = "Manager")]
        [HttpPut(Name = "UpdateStatusOrder")]
        public async Task<ActionResult<ApiResponse<DTOStatusOrders>>> UpdateAsync([FromBody] DTOStatusOrdersURequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var result = await _StatusOrder.UpdateStatusOrdersAsync(Request);
            return CreateResponse<DTOStatusOrders>(result!, StatusCodes.Status200OK, "Status Order Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
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
