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
        public async Task<ActionResult<ApiResponse<List<DTOMenuItem>>>> GetAllMenuItems([FromQuery] int page = 1)
        {
            try
            {
                if (page < 1)
                    return CreateResponse<List<DTOMenuItem>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");

                var menuItems = await _BusinessMenuItem.GetAllMenuItemsAsync(page);
                if (menuItems == null || menuItems.Count == 0)
                    return CreateResponse<List<DTOMenuItem>>(null!, StatusCodes.Status404NotFound, "No Menu Items found.");

                return CreateResponse(menuItems, StatusCodes.Status200OK, "Menu Items retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<List<DTOMenuItem>>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // ===================== GET BY ID =====================
        [HttpGet("GetMenuItemByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItem>>> GetMenuItemByID([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                    return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status400BadRequest, "Invalid Menu Item ID.");

                var menuItem = await _BusinessMenuItem.GetMenuItemByIdAsync(ID);
                if (menuItem == null)
                    return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status404NotFound, "Menu Item not found.");

                return CreateResponse(menuItem, StatusCodes.Status200OK, "Menu Item retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // ===================== ADD =====================
        [HttpPost("AddMenuItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItem>>> AddMenuItem([FromBody] DTOMenuItem menuItem)
        {
            try
            {
                if (menuItem == null)
                    return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status400BadRequest, "Menu Item data is null.");

                var businessMenuItem = await clsBusinessMenuItem.CreateBusinessMenuItemAsync(menuItem);

                bool isAdded = await businessMenuItem.SaveAsync();
                if (!isAdded)
                    return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status500InternalServerError, "Failed to add Menu Item.");

                return CreateResponse(businessMenuItem.MenuItem!, StatusCodes.Status201Created, "Menu Item added successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // ===================== UPDATE =====================
        [HttpPut("UpdateMenuItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOMenuItem>>> UpdateMenuItem([FromBody] DTOMenuItem menuItem)
        {
            try
            {
                if (menuItem == null || menuItem.MenuItemID <= 0)
                    return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status400BadRequest, "Invalid Menu Item data.");

                var existingItemDto = await _BusinessMenuItem.GetMenuItemByIdAsync(menuItem.MenuItemID);
                if (existingItemDto == null)
                    return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status404NotFound, "Menu Item not found.");

                // نسخ البيانات الجديدة
                existingItemDto.MenuItemName = menuItem.MenuItemName;
                existingItemDto.MenuItemDescription = menuItem.MenuItemDescription;
                existingItemDto.MenuItemPrice = menuItem.MenuItemPrice;
                existingItemDto.TypeItemID = menuItem.TypeItemID;
                existingItemDto.StatusMenuID = menuItem.StatusMenuID;
                existingItemDto.Image = menuItem.Image;

                var businessMenuItem = await clsBusinessMenuItem.CreateBusinessMenuItemAsync(existingItemDto, clsBusinessMenuItem.enMode.Update);

                bool isUpdated = await businessMenuItem.SaveAsync();
                if (!isUpdated)
                    return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status500InternalServerError, "Failed to update Menu Item.");

                return CreateResponse(businessMenuItem.MenuItem!, StatusCodes.Status200OK, "Menu Item updated successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOMenuItem>(null!, StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
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

                var existingItem = await _BusinessMenuItem.GetMenuItemByIdAsync(ID);
                if (existingItem == null)
                    return CreateResponse<string>(null!, StatusCodes.Status404NotFound, "Menu Item not found.");

                var businessMenuItem = await clsBusinessMenuItem.CreateBusinessMenuItemAsync(existingItem, clsBusinessMenuItem.enMode.Update
                );

                bool isDeleted = await businessMenuItem.DeleteAsync();
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
