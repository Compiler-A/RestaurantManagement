using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APILayer.Controllers
{
    [Route("api/APITypeItems")]
    [ApiController]
    public class APITypeItems : BaseController
    {
        private readonly IBusinessTypeItems _dataLayer;

        public APITypeItems(IBusinessTypeItems DataItem)
        {
            _dataLayer = DataItem;
        }

        // GET All
        [HttpGet("GetAllTypeItems", Name = "GetAllTypeItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTypeItems>>>> GetAllTypeItems([FromQuery] int page = 1)
        {
            try
            {
                if (page <= 0)
                    return CreateResponse<IEnumerable<DTOTypeItems>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than zero.");

                var typeItems = await _dataLayer.GetAllTypeItems(page);
                if (typeItems == null || typeItems.Count == 0)
                    return CreateResponse<IEnumerable<DTOTypeItems>>(null!, StatusCodes.Status404NotFound, "No TypeItems found.");

                return CreateResponse<IEnumerable<DTOTypeItems>>(typeItems, StatusCodes.Status200OK, "Completed!");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<IEnumerable<DTOTypeItems>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        // GET By ID
        [HttpGet("GetTypeItemById/{id}", Name = "GetTypeItemById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> GetTypeItemById([FromRoute] int id)
        {
            if (id <= 0)
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status400BadRequest, "Invalid TypeItem ID.");

            try
            {
                var typeItemDto = await _dataLayer.LoadByID(id);
                if (typeItemDto == null)
                    return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status404NotFound, "TypeItem not found.");

                return CreateResponse(typeItemDto, StatusCodes.Status200OK, "Completed");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        // POST Add
        [HttpPost("AddTypeItem", Name = "AddTypeItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> AddTypeItem([FromBody] DTOTypeItems typeItem)
        {
            if (typeItem == null)
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status400BadRequest, "TypeItem data is null.");

            try
            {
                var businessTypeItem = new clsBusinessTypeItems(typeItem, clsBusinessTypeItems.enMode.enAdd);
                bool isSaved = await businessTypeItem.Save();
                if (!isSaved)
                    return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status500InternalServerError, "A problem happened while handling your request.");

                return CreateResponse(typeItem, StatusCodes.Status201Created, "Add Successfully");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        // PUT Update
        [HttpPut("UpdateTypeItem", Name = "UpdateTypeItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> UpdateTypeItem([FromBody] DTOTypeItems typeItem)
        {
            if (typeItem == null || typeItem.TypeItemID <= 0)
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status400BadRequest, "Invalid TypeItem data.");

            try
            {
                var businessTypeItem = new clsBusinessTypeItems(typeItem, clsBusinessTypeItems.enMode.enUpdate);
                bool isSaved = await businessTypeItem.Save();

                if (!isSaved)
                    return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status500InternalServerError, "A problem happened while handling your request.");

                return CreateResponse(typeItem, StatusCodes.Status200OK, "Update successfully");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        // DELETE
        [HttpDelete("DeleteTypeItem/{id}", Name = "DeleteTypeItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> DeleteTypeItem([FromRoute] int id)
        {
            if (id <= 0)
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status400BadRequest, "Invalid TypeItem ID.");

            try
            {
                var typeItem = await _dataLayer.LoadByID(id);
                if (typeItem == null)
                    return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status404NotFound, "TypeItem not found.");

                var businessTypeItem = new clsBusinessTypeItems( typeItem, clsBusinessTypeItems.enMode.enUpdate);
                bool isDeleted = await businessTypeItem.Delete();
                if (isDeleted)
                    return CreateResponse(typeItem, StatusCodes.Status200OK, "Deleted successfully");

                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status500InternalServerError, "A problem happened while handling your request.");
            }
            catch (System.Exception ex)
            {
                return CreateResponse<DTOTypeItems>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}
