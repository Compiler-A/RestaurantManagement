using APILayer.Filters;
using ContractsLayerRestaurant.Interfaces.Services;
using ContractsLayerRestaurant.DTORequest.MenuItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ContractsLayerRestaurant.DTOResponse;
using BusinessLayerRestaurant.Mapper;



namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/MenuItems")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIMenuItems : BaseController
    {
        
        IMenuItemsService _BusinessMenuItem;
        public APIMenuItems(IMenuItemsService b)
        {
            _BusinessMenuItem = b;
        }

        [AllowAnonymous]
        [HttpGet(Name ="GetAllMenuItems")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItemResponse>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var menuItems = await _BusinessMenuItem.GetAllAsync(page);
            var listResponse = menuItems.Select(m => m.ToResponse()).ToList();
            return CreateResponse(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [HttpGet("all-availables")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItemResponse>>>> GetAllAvailablesAsync()
        {
            var menuItems = await _BusinessMenuItem.GetAllAvailablesAsync();
            var listResponse = menuItems.Select(m => m.ToResponse()).ToList();
            return CreateResponse(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");
        }

        [AllowAnonymous]
        [HttpGet("all-filters")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItemResponse>>>> GetAllFiltersAsync([FromQuery] DTOMenuItemsFilterRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var menuItems = await _BusinessMenuItem.GetAllFiltersAsync(Request);
            var listResponse = menuItems.Select(m => m.ToResponse()).ToList();
            return CreateResponse<List<DTOMenuItemResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name ="GetMenuItemByID")]
        public async Task<ActionResult<ApiResponse<DTOMenuItemResponse>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            var menuItem = await _BusinessMenuItem.GetAsync(ID);
            return CreateResponse<DTOMenuItemResponse>(menuItem!.ToResponse(), StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = $"{RoleNames.Manager}, {RoleNames.Chef}")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name ="AddMenuItem")]
        public async Task<ActionResult<ApiResponse<DTOMenuItemResponse>>> CreateAsync([FromBody] DTOMenuItemsCRequest menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _BusinessMenuItem.CreateAsync(menuItem);
            return CreatedAtRoute("GetMenuItemByID", new { ID = dto!.ItemID }, dto.ToResponse());

        }


        [Authorize(Roles = $"{RoleNames.Manager}, {RoleNames.Chef},{RoleNames.SousChef}")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name ="UpdateMenuItem")]
        public async Task<ActionResult<ApiResponse<DTOMenuItemResponse>>> UpdateAsync([FromBody] DTOMenuItemsURequest menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _BusinessMenuItem.UpdateAsync(menuItem);
            return CreateResponse<DTOMenuItemResponse>(dto!.ToResponse(), StatusCodes.Status200OK, "Menu Item Updated Successfully!");
        }

        [Authorize(Roles = $"{RoleNames.Manager}, {RoleNames.Chef}")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            bool isDeleted = await _BusinessMenuItem.DeleteAsync(ID);
            return CreateResponse(isDeleted ,StatusCodes.Status200OK, "Menu Item Deleted Successfully!");
        }
    }
}
