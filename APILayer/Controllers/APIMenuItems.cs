using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.MenuItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;



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
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var menuItems = await _BusinessMenuItem.GetAllMenuItemsAsync(page);
            return CreateResponse(menuItems, StatusCodes.Status200OK, $"Row: {menuItems.Count}");

        }

        [AllowAnonymous]
        [HttpGet("all-availables")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllAvailablesAsync()
        {
            var menuItems = await _BusinessMenuItem.GetAllMenuItemsAvailablesAsync();
            return CreateResponse(menuItems, StatusCodes.Status200OK, $"Row: {menuItems.Count}");
        }

        [AllowAnonymous]
        [HttpGet("all-filters")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllFiltersAsync([FromQuery] DTOMenuItemsFilterRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var menuItems = await _BusinessMenuItem.GetAllMenuItemsFiltersAsync(Request);
            return CreateResponse<List<DTOMenuItems>>(menuItems!, StatusCodes.Status200OK, $"Row: {menuItems.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name ="GetMenuItemByID")]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            var menuItem = await _BusinessMenuItem.GetMenuItemAsync(ID);
            return CreateResponse<DTOMenuItems>(menuItem!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager,Chef")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name ="AddMenuItem")]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> CreateAsync([FromBody] DTOMenuItemsCRequest menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _BusinessMenuItem.AddMenuItemAsync(menuItem);
            return CreatedAtRoute("GetMenuItemByID", new { ID = dto!.ID }, dto);

        }


        [Authorize(Roles = "Manager,Chef,Sous Chef")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name ="UpdateMenuItem")]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> UpdateAsync([FromBody] DTOMenuItemsURequest menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _BusinessMenuItem.UpdateMenuItemAsync(menuItem);
            return CreateResponse<DTOMenuItems>(dto!, StatusCodes.Status200OK, "Menu Item Updated Successfully!");


        }

        [Authorize(Roles = "Manager,Chef")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            bool isDeleted = await _BusinessMenuItem.DeleteMenuItemAsync(ID);
            return CreateResponse(isDeleted ,StatusCodes.Status200OK, "Menu Item Deleted Successfully!");
        }
    }
}
