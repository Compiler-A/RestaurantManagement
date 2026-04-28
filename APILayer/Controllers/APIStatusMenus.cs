using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.StatusMenus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using DomainLayer.Entities;


namespace APILayer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/StatusMenus")]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIStatusMenus : BaseController
    {
        private readonly IStatusMenusService _dataStatusMenus;

        public APIStatusMenus(IStatusMenusService dataStatusMenus)
        {
            _dataStatusMenus = dataStatusMenus;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetAllStatusMenus")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<List<StatusMenu>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var list = await _dataStatusMenus.GetAllAsync(page);
            return CreateResponse(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetStatusMenuByID")]
        public async Task<ActionResult<ApiResponse<StatusMenu>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            var resource = await _dataStatusMenus.GetAsync(ID);
            return CreateResponse<StatusMenu>(resource!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddStatusMenu")]
        public async Task<ActionResult<ApiResponse<StatusMenu>>> CreateAsync([FromBody] DTOStatusMenusCRequest statusMenu)
        {
            if (statusMenu == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _dataStatusMenus.CreateAsync(statusMenu);
            return CreatedAtRoute("GetStatusMenuByID", new { ID = dto!.ID }, dto);

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateStatusMenu")]
        public async Task<ActionResult<ApiResponse<StatusMenu>>> UpdateAsync([FromBody] DTOStatusMenusURequest statusMenu)
        {
            if (statusMenu == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _dataStatusMenus.UpdateAsync(statusMenu);
            return CreateResponse<StatusMenu>(dto!, StatusCodes.Status200OK, "Status Menu Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStatusMenu(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            bool isDeleted = await _dataStatusMenus.DeleteAsync(ID);
            return CreateResponse<bool>(isDeleted, StatusCodes.Status200OK, "Status Menu Deleted Successfully!");


        }
    }
}
