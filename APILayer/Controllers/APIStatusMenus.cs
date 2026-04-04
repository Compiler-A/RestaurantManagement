using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace APILayer.Controllers
{
    [ApiController]
    [Route("api/StatusMenus")]
    [ValidateModel]
    public class APIStatusMenus : BaseController
    {
        private readonly IBusinessStatusMenus _dataStatusMenus;

        public APIStatusMenus(IBusinessStatusMenus dataStatusMenus)
        {
            _dataStatusMenus = dataStatusMenus;
        }


        [HttpGet(Name = "GetAllStatusMenus")]
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
            return CreateResponse(list, StatusCodes.Status200OK, "Status Menus retrieved successfully.");

        }

        [HttpGet("{ID}", Name = "GetStatusMenuByID")]
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
            return CreateResponse<DTOStatusMenus>(resource!, StatusCodes.Status200OK, "Status Menu retrieved successfully.");
        }

        [HttpPost(Name = "AddStatusMenu")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
            return CreateResponse<DTOStatusMenus>(dto!, StatusCodes.Status200OK, "Status Menu updated successfully.");

        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
            return CreateResponse<bool>(isDeleted, StatusCodes.Status200OK, "Status Menu Deleted Saccessfully!");


        }
    }
}
