using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using DataLayerRestaurant;
using BusinessLayerRestaurant;
using APILayer.Filters;

namespace APILayer.Controllers
{
    [Route("api/Settings")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class APISettings : BaseController
    {
        private readonly IBusinessSettings _BusinessSettings;
       
        public APISettings(IBusinessSettings s)
        {
            _BusinessSettings = s;
        }

        [HttpGet(Name = "GetAllSettings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOSettings>>>> GetAllAsync([FromQuery] int page = 1)
        {

            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }
            var list = await _BusinessSettings.GetAllSettingsAsync(page);
            return CreateResponse<IEnumerable<DTOSettings>>(list, StatusCodes.Status200OK, $"Row: {list.Count}");
        }

        [HttpGet("{ID}", Name = "GetSettingByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> GetByIDAsync([FromRoute] int ID = 1)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var DTO = await _BusinessSettings.GetSettingAsync(ID);
            return CreateResponse<DTOSettings>(DTO!, StatusCodes.Status200OK, "Success");
        }

        [HttpPost(Name = "AddSetting")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> CreateAsync([FromBody] DTOSettingsCRequest Setting)
        {
            if (Setting == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var success = await _BusinessSettings.AddSettingAsync(Setting);
            return CreatedAtRoute("GetSettingByID", new { ID = success!.ID }, success);

        }

        [HttpPut(Name = "UpdateSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> UpdateAsync([FromBody] DTOSettingsURequest Setting)
        {
            if (Setting == null)
            {
                throw new ArgumentNullException("Request is null!");
            }


            var success = await _BusinessSettings.UpdateSettingAsync(Setting);
            return CreateResponse<DTOSettings>(success!, StatusCodes.Status200OK, "Setting updated successfully.");

        }

        [HttpDelete("{ID}", Name = "DeleteSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be greater than 0.");
            }
            var success = await _BusinessSettings.DeleteSettingAsync(ID);
            return CreateResponse<bool>(success, StatusCodes.Status200OK, "Setting deleted successfully.");

        }
    }
}
