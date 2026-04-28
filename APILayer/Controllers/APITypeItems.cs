using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.TypeItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;


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
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        [HttpGet(Name = "GetAllTypeItems")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TypeItem>>>> GetAllAsync([FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var typeItems = await _dataLayer.GetAllAsync(page);
            return CreateResponse<IEnumerable<TypeItem>>(typeItems, StatusCodes.Status200OK, $"Row: {typeItems.Count}");

        }

        [AllowAnonymous]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetTypeItemById")]
        public async Task<ActionResult<ApiResponse<TypeItem>>> GetByIDAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");
            }
            var typeItemDto = await _dataLayer.GetAsync(ID);
            return CreateResponse(typeItemDto!, StatusCodes.Status200OK, "Found Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddTypeItem")]
        public async Task<ActionResult<ApiResponse<TypeItem>>> CreateAsync([FromBody] DTOTypeItemsCRequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");



            var dto = await _dataLayer.CreateAsync(typeItem);
            return CreatedAtRoute("GetTypeItemById", new { ID = dto!.ID }, dto);

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateTypeItem")]
        public async Task<ActionResult<ApiResponse<TypeItem>>> UpdateAsync([FromBody] DTOTypeItemsURequest typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException("Request is null!");



            var dto = await _dataLayer.UpdateAsync(typeItem);
            return CreateResponse(dto!, StatusCodes.Status200OK, "Type Item Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}", Name = "DeleteTypeItem")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
                throw new ArgumentOutOfRangeException("ID number must be greater than 0.");

            var typeItem = await _dataLayer.DeleteAsync(ID);
            return CreateResponse<bool>(true, StatusCodes.Status200OK, "Type Item Deleted Successfully!");
        }
    }
}
