using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DomainLayer.Entities;


namespace APILayer.Controllers
{
    [Authorize]
    [Route("api/Settings")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APISettings : BaseController
    {
        private readonly ISettingsService _BusinessSettings;
       
        public APISettings(ISettingsService s)
        {
            _BusinessSettings = s;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(Name = "GetAllSettings")]
        [EnableRateLimiting(NameRateLimitPolicies.GetAll)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Setting>>>> GetAllAsync([FromQuery] int page = 1)
        {

            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await _BusinessSettings.GetAllAsync(page);
            return CreateResponse<IEnumerable<Setting>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.GetOne)]
        [HttpGet("{ID}", Name = "GetSettingByID")]
        public async Task<ActionResult<ApiResponse<Setting>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var DTO = await _BusinessSettings.GetAsync(ID);
            return CreateResponse<Setting>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Add)]
        [HttpPost(Name = "AddSetting")]
        public async Task<ActionResult<ApiResponse<Setting>>> CreateAsync([FromBody] DTOSettingsCRequest Setting)
        {
            if (Setting == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var success = await _BusinessSettings.CreateAsync(Setting);
            return CreatedAtRoute("GetSettingByID", new { ID = success!.ID }, success);

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Update)]
        [HttpPut(Name = "UpdateSetting")]
        public async Task<ActionResult<ApiResponse<Setting>>> UpdateAsync([FromBody] DTOSettingsURequest Setting)
        {
            if (Setting == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var success = await _BusinessSettings.UpdateAsync(Setting);
            return CreateResponse<Setting>(success!, StatusCodes.Status200OK, "Setting Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [EnableRateLimiting(NameRateLimitPolicies.Delete)]
        [HttpDelete("{ID}", Name = "DeleteSetting")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var success = await _BusinessSettings.DeleteAsync(ID);
            return CreateResponse<bool>(success, StatusCodes.Status200OK, "Setting Deleted Successfully!");

        }
    }
}
