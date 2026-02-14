using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APILayer.Controllers
{
    [ApiController]
    [Route("api/APIStatusMenus")]
    public class APIStatusMenus : BaseController
    {
        private readonly IBusinessStatusMenus _dataStatusMenus;

        public APIStatusMenus(IBusinessStatusMenus dataStatusMenus)
        {
            _dataStatusMenus = dataStatusMenus;
        }


        // ===================== GET ALL =====================
        [HttpGet("GetAllStatusMenus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOStatusMenus>>>> GetAllStatusMenus([FromQuery] int page = 1)
        {
            try
            {
                if (page < 1)
                    return CreateResponse<List<DTOStatusMenus>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");

                
                var list = await _dataStatusMenus.GetAll(page);

                if (list == null || list.Count == 0)
                    return CreateResponse<List<DTOStatusMenus>>(null!, StatusCodes.Status404NotFound, "No Status Menus found.");

                return CreateResponse(list, StatusCodes.Status200OK, "Status Menus retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<List<DTOStatusMenus>>(null!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ===================== GET BY ID =====================
        [HttpGet("GetStatusMenuByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> GetStatusMenuByID(int id)
        {
            try
            {
                if (id <= 0)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status400BadRequest, "Invalid ID.");

                var resource = await _dataStatusMenus.LoadByID(id);

                if (resource == null)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status404NotFound, "Status Menu not found.");

                return CreateResponse(resource, StatusCodes.Status200OK, "Status Menu retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ===================== ADD =====================
        [HttpPost("AddStatusMenu")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> AddStatusMenu([FromBody] DTOStatusMenus statusMenu)
        {
            try
            {
                if (statusMenu == null)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status400BadRequest, "Status Menu data is null.");

                var business = new clsBusinessStatusMenus(statusMenu, clsBusinessStatusMenus.enMode.enAdd);

                bool isAdded = await business.Save();

                if (!isAdded)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status500InternalServerError, "Failed to add Status Menu.");

                return CreateResponse(business.StatusMenus!, StatusCodes.Status201Created, "Status Menu added successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ===================== UPDATE =====================
        [HttpPut("UpdateStatusMenu")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> UpdateStatusMenu([FromBody] DTOStatusMenus statusMenu)
        {
            try
            {
                if (statusMenu == null || statusMenu.StatusMenuID <= 0)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status400BadRequest, "Invalid Status Menu data.");

                var business = new clsBusinessStatusMenus(statusMenu, clsBusinessStatusMenus.enMode.enUpdate);

                bool isUpdated = await business.Save();

                if (!isUpdated)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status500InternalServerError, "Failed to update Status Menu.");

                return CreateResponse(business.StatusMenus!, StatusCodes.Status200OK, "Status Menu updated successfully.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ===================== DELETE =====================
        [HttpDelete("DeleteStatusMenu/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteStatusMenu(int id)
        {
            try
            {
                if (id <= 0)
                    return CreateResponse<string>(null!, StatusCodes.Status400BadRequest, "Invalid ID.");

                bool isDeleted = await _dataStatusMenus.Delete(id);

                if (!isDeleted)
                    return CreateResponse<string>(null!, StatusCodes.Status404NotFound, "Status Menu not found or already deleted.");

                return CreateResponse("Status Menu deleted successfully.", StatusCodes.Status200OK);
            }
            catch (System.Exception ex)
            {
                return CreateResponse<string>(null!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
