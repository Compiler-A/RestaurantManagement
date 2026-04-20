using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.TypeItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/TypeItems")]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APITypeItems : BaseController
    {
        private readonly ITypeItemsService _dataLayer;

        public APITypeItems(ITypeItemsService DataItem)
        {
            _dataLayer = DataItem;
        }

        [AllowAnonymous]
        [EnableRateLimiting("GetAllLimiter")]
        [HttpGet(Name = "GetAllTypeItems")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTypeItems>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var typeItems = await _dataLayer.GetAllTypeItemsAsync(page);
            return CreateResponse<IEnumerable<DTOTypeItems>>(typeItems, StatusCodes.Status200OK, $"Row: {typeItems.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting("GetOneLimiter")]
        [HttpGet("{ID}", Name = "GetTypeItemById")]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var typeItemDto = await _dataLayer.GetTypeItemAsync(ID);
            return CreateResponse(typeItemDto!, StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting("AddLimiter")]
        [HttpPost(Name = "AddTypeItem")]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> CreateAsync([FromBody] DTOTypeItemsCRequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");



            var dto = await _dataLayer.AddTypeItemAsync(typeItem);
            return CreatedAtRoute("GetTypeItemById", new { ID = dto!.ID }, dto);

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting("UpdateLimiter")]
        [HttpPut(Name = "UpdateTypeItem")]
        public async Task<ActionResult<ApiResponse<DTOTypeItems>>> UpdateAsync([FromBody] DTOTypeItemsURequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");



            var dto = await _dataLayer.UpdateTypeItemAsync(typeItem);
            return CreateResponse(dto!, StatusCodes.Status200OK, "Type Item Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting("DeleteLimiter")]
        [HttpDelete("{ID}", Name = "DeleteTypeItem")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");

            var typeItem = await _dataLayer.GetTypeItemAsync(ID);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Type Item Deleted Successfully!");
        }
    }
}
