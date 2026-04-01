using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace APILayer.Controllers
{
    [Route("api/MenuItems")]
    [ApiController]
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
            try
            {
                if (page < 1)
                    return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");

                var menuItems = await _BusinessMenuItem.GetAllMenuItemsAsync(page);
                if (menuItems == null || menuItems.Count == 0)
                    return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status404NotFound, "No Menu Items found.");

                return CreateResponse(menuItems, StatusCodes.Status200OK, "Menu Items retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("all-availables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllAvailablesAsync()
        {
            try
            {
                var menuItems = await _BusinessMenuItem.GetAllMenuItemsAvailablesAsync();
                if (menuItems == null || menuItems.Count == 0)
                    return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status404NotFound, "No Menu Items found.");

                return CreateResponse(menuItems, StatusCodes.Status200OK, "Menu Items retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("all-filters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllFiltersAsync([FromQuery] int page = 1, [FromQuery] int StatusMenuID = -1, [FromQuery] int TypeItemID = -1)
        {
            try
            {
                if (page < 1)
                    return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");

                var menuItems = await _BusinessMenuItem.GetAllMenuItemsFiltersAsync(page, StatusMenuID, TypeItemID);

                if (menuItems == null || menuItems.Count == 0)
                    return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status404NotFound, "No Menu Items found with the specified filters.");
                return CreateResponse(menuItems, StatusCodes.Status200OK, "Filtered Menu Items retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<List<DTOMenuItems>>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{ID}", Name ="GetMenuItemByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> GetByIDAsync([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status400BadRequest, "Invalid Menu Item ID.");

                var menuItem = await _BusinessMenuItem.GetMenuItemAsync(ID);
                if (menuItem == null)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status404NotFound, "Menu Item not found.");

                return CreateResponse(menuItem, StatusCodes.Status200OK, "Menu Item retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // ===================== ADD =====================
        [HttpPost(Name ="AddMenuItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> CreateAsync([FromBody] DTOMenuItemsCRequest menuItem)
        {
            try
            {
                if (menuItem == null)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status400BadRequest, "Menu Item data is null.");



                var dto = await _BusinessMenuItem.AddMenuItemAsync(menuItem);
                if (dto == null)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status500InternalServerError, "Failed to add Menu Item.");

                return CreatedAtRoute("GetMenuItemByID", new { ID = dto.ID}, dto);
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // ===================== UPDATE =====================
        [HttpPut(Name ="UpdateMenuItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> UpdateAsync([FromBody] DTOMenuItemsURequest menuItem)
        {
            try
            {
                if (menuItem == null || menuItem.ID <= 0)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status400BadRequest, "Invalid Menu Item data.");

                var existingItemDto = await _BusinessMenuItem.GetMenuItemAsync(menuItem.ID);
                if (existingItemDto == null)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status404NotFound, "Menu Item not found.");



                var dto = await _BusinessMenuItem.UpdateMenuItemAsync(menuItem);

                if (dto == null)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status500InternalServerError, "Failed to update Menu Item.");

                return CreateResponse(dto!, StatusCodes.Status200OK, "Menu Item updated successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // ===================== DELETE =====================
        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteAsync([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                    return CreateResponse<string>(null!, StatusCodes.Status400BadRequest, "Invalid Menu Item ID.");

                var existingItem = await _BusinessMenuItem.GetMenuItemAsync(ID);
                if (existingItem == null)
                    return CreateResponse<string>(null!, StatusCodes.Status404NotFound, "Menu Item not found.");

                

                bool isDeleted = await _BusinessMenuItem.DeleteMenuItemAsync(ID);
                if (!isDeleted)
                    return CreateResponse<string>(null!, StatusCodes.Status500InternalServerError, "Failed to delete Menu Item.");

                return CreateResponse("Menu Item deleted successfully.", StatusCodes.Status200OK);
            }
            catch (System.Exception ex)
            {
                return CreateResponse<string>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
