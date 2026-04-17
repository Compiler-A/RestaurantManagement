using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.StatusMenus;
using Microsoft.AspNetCore.Authorization;



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


        [HttpGet(Name = "GetAllStatusMenus")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<DTOStatusMenus>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            var list = await _dataStatusMenus.GetAllStatusMenusAsync(page);
            return CreateResponse(list, StatusCodes.Status200OK, $"Row: {list.Count}");

        }

        [HttpGet("{ID}", Name = "GetStatusMenuByID")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            var resource = await _dataStatusMenus.GetStatusMenuAsync(ID);
            return CreateResponse<DTOStatusMenus>(resource!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [HttpPost(Name = "AddStatusMenu")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> CreateAsync([FromBody] DTOStatusMenusCRequest statusMenu)
        {
            if (statusMenu == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _dataStatusMenus.AddStatusMenuAsync(statusMenu);
            return CreatedAtRoute("GetStatusMenuByID", new { ID = dto!.ID }, dto);

        }

        [HttpPut(Name = "UpdateStatusMenu")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOStatusMenus>>> UpdateAsync([FromBody] DTOStatusMenusURequest statusMenu)
        {
            if (statusMenu == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var dto = await _dataStatusMenus.UpdateStatusMenuAsync(statusMenu);
            return CreateResponse<DTOStatusMenus>(dto!, StatusCodes.Status200OK, "Status Menu Updated Successfully!");

        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStatusMenu(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }

            bool isDeleted = await _dataStatusMenus.DeleteStatusMenuAsync(ID);
            return CreateResponse<bool>(isDeleted, StatusCodes.Status200OK, "Status Menu Deleted Successfully!");


        }
    }
}
