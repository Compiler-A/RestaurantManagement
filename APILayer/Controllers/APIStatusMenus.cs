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

                
                var list = await _dataStatusMenus.GetAllStatusMenusAsync(page);

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

                var resource = await _dataStatusMenus.GetStatusMenuAsync(id);

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
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> AddStatusMenu([FromBody] DTOStatusMenusCRequest statusMenu)
        {
            try
            {
                if (statusMenu == null)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status400BadRequest, "Status Menu data is null.");


                var dto = await _dataStatusMenus.AddStatusMenuAsync(statusMenu);
                if (dto == null)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status500InternalServerError, "Failed to add Status Menu.");

                return CreateResponse(dto!, StatusCodes.Status201Created, "Status Menu added successfully.");
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
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> UpdateStatusMenu([FromBody] DTOStatusMenusURequest statusMenu)
        {
            try
            {
                if (statusMenu == null || statusMenu.ID <= 0)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status400BadRequest, "Invalid Status Menu data.");


                var dto = await _dataStatusMenus.UpdateStatusMenuAsync(statusMenu);
                if (dto == null)
                    return CreateResponse<DTOStatusMenus>(null!, StatusCodes.Status500InternalServerError, "Failed to update Status Menu.");

                return CreateResponse(dto!, StatusCodes.Status200OK, "Status Menu updated successfully.");
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
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStatusMenu(int id)
        {
            try
            {
                if (id <= 0)
                    return CreateResponse<bool>(false!, StatusCodes.Status400BadRequest, "Invalid ID.");

                bool isDeleted = await _dataStatusMenus.DeleteStatusMenuAsync(id);

                if (!isDeleted)
                    return CreateResponse<bool>(false!, StatusCodes.Status404NotFound, "Status Menu not found or already deleted.");

                return CreateResponse<bool>(true, StatusCodes.Status200OK, "Status Menu Deleted Saccessfully!");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<bool>(false!, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
