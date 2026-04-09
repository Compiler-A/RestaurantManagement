using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.MenuItems;



namespace APILayer.Controllers
{
    [Route("api/MenuItems")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APIMenuItems : BaseController
    {
        
        IBusinessMenuItems _BusinessMenuItem;
        public APIMenuItems(IBusinessMenuItems b)
        {
            _BusinessMenuItem = b;
        }

        

        [HttpGet(Name ="GetAllMenuItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var menuItems = await _BusinessMenuItem.GetAllMenuItemsAsync(page);
            return CreateResponse(menuItems, StatusCodes.Status200OK, "Menu Items retrieved successfully.");

        }


        [HttpGet("all-availables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllAvailablesAsync()
        {
            var menuItems = await _BusinessMenuItem.GetAllMenuItemsAvailablesAsync();
            return CreateResponse(menuItems, StatusCodes.Status200OK, "Menu Items retrieved successfully.");
        }


        [HttpGet("all-filters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllFiltersAsync([FromQuery] DTOMenuItemsFilterRequest Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var menuItems = await _BusinessMenuItem.GetAllMenuItemsFiltersAsync(Request);
            return CreateResponse<List<DTOMenuItems>>(menuItems!, StatusCodes.Status200OK, "Filtered Menu Items retrieved successfully.");

        }

        [HttpGet("{ID}", Name ="GetMenuItemByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            var menuItem = await _BusinessMenuItem.GetMenuItemAsync(ID);
            return CreateResponse<DTOMenuItems>(menuItem!, StatusCodes.Status200OK, "Menu Item retrieved successfully.");
        }

        [HttpPost(Name ="AddMenuItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> CreateAsync([FromBody] DTOMenuItemsCRequest menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _BusinessMenuItem.AddMenuItemAsync(menuItem);
            return CreatedAtRoute("GetMenuItemByID", new { ID = dto!.ID }, dto);

        }


        [HttpPut(Name ="UpdateMenuItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> UpdateAsync([FromBody] DTOMenuItemsURequest menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _BusinessMenuItem.UpdateMenuItemAsync(menuItem);
            return CreateResponse<DTOMenuItems>(dto!, StatusCodes.Status200OK, "Menu Item updated successfully.");


        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            bool isDeleted = await _BusinessMenuItem.DeleteMenuItemAsync(ID);
            return CreateResponse(isDeleted ,StatusCodes.Status200OK, "Menu Item deleted successfully.");
        }
    }
}
