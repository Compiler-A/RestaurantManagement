using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace APILayer.Controllers
{
    [Route("api/APIMenuItems")]
    [ApiController]
    public class APIMenuItems : BaseController
    {
        
        IBusinessMenuItems _BusinessMenuItem;
        public APIMenuItems(IBusinessMenuItems b)
        {
            _BusinessMenuItem = b;
        }

        

        // ===================== GET ALL =====================
        [HttpGet("GetAllMenuItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllMenuItems([FromQuery] int page = 1)
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
        [HttpGet("GetAllMenuItemsAvailables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetAllMenuItemsAvailables()
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


        [HttpGet("GetFilterAllMenuItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOMenuItems>>>> GetFilterAllMenuItems([FromQuery] int page = 1, [FromQuery] int StatusMenuID = -1, [FromQuery] int TypeItemID = -1)
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

        // ===================== GET BY ID =====================
        [HttpGet("GetMenuItemByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> GetMenuItemByID([FromRoute] int ID)
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
        [HttpPost("AddMenuItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> AddMenuItem([FromBody] DTOMenuItemsCRequest menuItem)
        {
            try
            {
                if (menuItem == null)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status400BadRequest, "Menu Item data is null.");



                var dto = await _BusinessMenuItem.AddMenuItemAsync(menuItem);
                if (dto == null)
                    return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status500InternalServerError, "Failed to add Menu Item.");

                return CreateResponse(dto!, StatusCodes.Status201Created, "Menu Item added successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOMenuItems>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // ===================== UPDATE =====================
        [HttpPut("UpdateMenuItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItems>>> UpdateMenuItem([FromBody] DTOMenuItemsURequest menuItem)
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
        [HttpDelete("DeleteMenuItem/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteMenuItem([FromRoute] int ID)
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
