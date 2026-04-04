using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APILayer.Controllers
{
    [Route("api/TypeItems")]
    [ApiController]
    public class APITypeItems : BaseController
    {
        private readonly IBusinessTypeItems _dataLayer;

        public APITypeItems(IBusinessTypeItems DataItem)
        {
            _dataLayer = DataItem;
        }

        [HttpGet(Name = "GetAllTypeItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTypeItems>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var typeItems = await _dataLayer.GetAllTypeItemsAsync(page);
            return CreateResponse<IEnumerable<DTOTypeItems>>(typeItems, StatusCodes.Status200OK, "Completed!");

        }

        [HttpGet("{ID}", Name = "GetTypeItemById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var typeItemDto = await _dataLayer.GetTypeItemAsync(ID);
            return CreateResponse(typeItemDto!, StatusCodes.Status200OK, "Completed");

        }

        [HttpPost(Name = "AddTypeItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> CreateAsync([FromBody] DTOTypeItemsCRequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");



            var dto = await _dataLayer.AddTypeItemAsync(typeItem);
            return CreatedAtRoute("GetTypeItemById", new { ID = dto!.ID }, dto);

        }

        [HttpPut(Name = "UpdateTypeItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> UpdateAsync([FromBody] DTOTypeItemsURequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");



            var dto = await _dataLayer.UpdateTypeItemAsync(typeItem);
            return CreateResponse(dto!, StatusCodes.Status200OK, "Update successfully");

        }

        [HttpDelete("{ID}", Name = "DeleteTypeItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");

            var typeItem = await _dataLayer.GetTypeItemAsync(ID);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Deleted successfully");
        }
    }
}
