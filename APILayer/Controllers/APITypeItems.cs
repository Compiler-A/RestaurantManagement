using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.TypeItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ContractsLayerRestaurant.DTOResponse;
using BusinessLayerRestaurant.Mapper;


namespace APILayer.Controllers
{
    [Authorize]
    [ApiController]
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
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet(Name = "GetAllTypeItems")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOTypeItemResponse>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var typeItems = await _dataLayer.GetAllAsync(page);
            var listResponse = typeItems.Select(x => x.ToResponse()).ToList();
            return CreateResponse<IEnumerable<DTOTypeItemResponse>>(listResponse, StatusCodes.Status200OK, $"Row: {listResponse.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetTypeItemById")]
        public async Task<ActionResult<ApiResponse<DTOTypeItemResponse>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var typeItemDto = await _dataLayer.GetAsync(ID);
            return CreateResponse<DTOTypeItemResponse>(typeItemDto!.ToResponse(), StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddTypeItem")]
        public async Task<ActionResult<ApiResponse<DTOTypeItemResponse>>> CreateAsync([FromBody] DTOTypeItemsCRequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");



            var dto = await _dataLayer.CreateAsync(typeItem);
            return CreatedAtRoute("GetTypeItemById", new { ID = dto!.TypeItemID }, dto.ToResponse());

        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateTypeItem")]
        public async Task<ActionResult<ApiResponse<DTOTypeItemResponse>>> UpdateAsync([FromBody] DTOTypeItemsURequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");

            var dto = await _dataLayer.UpdateAsync(typeItem);
            return CreateResponse<DTOTypeItemResponse>(dto!.ToResponse(), StatusCodes.Status200OK, "Type Item Updated Successfully!");

        }

        [Authorize(Roles = $"{RoleNames.Manager}")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}", Name = "DeleteTypeItem")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");

            var typeItem = await _dataLayer.DeleteAsync(ID);
            return CreateResponse<bool>(typeItem, StatusCodes.Status200OK, "Type Item Deleted Successfully!");
        }
    }
}
