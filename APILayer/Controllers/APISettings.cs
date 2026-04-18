using APILayer.Filters;
using Microsoft.AspNetCore.Mvc;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Settings;
using Microsoft.AspNetCore.Authorization;


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
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOSettings>>>> GetAllAsync([FromQuery] int page = 1)
        {

            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await _BusinessSettings.GetAllSettingsAsync(page);
            return CreateResponse<IEnumerable<DTOSettings>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("{ID}", Name = "GetSettingByID")]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var DTO = await _BusinessSettings.GetSettingAsync(ID);
            return CreateResponse<DTOSettings>(DTO!, StatusCodes.Status200OK, "Found Successfully!");
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(Name = "AddSetting")]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> CreateAsync([FromBody] DTOSettingsCRequest Setting)
        {
            if (Setting == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var success = await _BusinessSettings.AddSettingAsync(Setting);
            return CreatedAtRoute("GetSettingByID", new { ID = success!.ID }, success);

        }

        [Authorize(Roles = "Manager")]
        [HttpPut(Name = "UpdateSetting")]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> UpdateAsync([FromBody] DTOSettingsURequest Setting)
        {
            if (Setting == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var success = await _BusinessSettings.UpdateSettingAsync(Setting);
            return CreateResponse<DTOSettings>(success!, StatusCodes.Status200OK, "Setting Updated Successfully!");

        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{ID}", Name = "DeleteSetting")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var success = await _BusinessSettings.DeleteSettingAsync(ID);
            return CreateResponse<bool>(success, StatusCodes.Status200OK, "Setting Deleted Successfully!");

        }
    }
}
