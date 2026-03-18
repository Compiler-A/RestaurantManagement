using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using DataLayerRestaurant;
using BusinessLayerRestaurant;

namespace APILayer.Controllers
{
    [Route("api/APISettings")]
    [ApiController]
    public class APISettings : BaseController
    {
        private readonly IBusinessSettings _BusinessSettings;
       
        public APISettings(IBusinessSettings s)
        {
            _BusinessSettings = s;
        }

        [HttpGet("GetAllSettings", Name = "GetAllSettings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<DTOSettings>>>> GetAllSettings([FromQuery] int page = 1)
        {
            try
            {
                if (page <= 0)
                {
                    return CreateResponse<IEnumerable<DTOSettings>>(null!, StatusCodes.Status400BadRequest, "Page number must be greater than 0.");
                }
                var list = await _BusinessSettings.GetAllSettingsAsync(page);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOSettings>>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOSettings>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOSettings>>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetSetting/{ID}", Name = "GetSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> GetSetting([FromRoute] int ID = 1)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<DTOSettings>(null!, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var DTO = await _BusinessSettings.GetSettingAsync(ID);
                if (DTO == null)
                {
                    return CreateResponse<DTOSettings>(null!, StatusCodes.Status404NotFound, "Not Found!");
                }
                return CreateResponse<DTOSettings>(DTO, StatusCodes.Status200OK, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOSettings>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddSetting", Name = "AddSetting")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> AddSetting([FromBody] DTOSettingsCRequest Setting)
        {
            try
            {
                if (Setting == null)
                {
                    return CreateResponse<DTOSettings>(null!, StatusCodes.Status400BadRequest, "Employee data is null.");
                }
                
                _BusinessSettings.CreateRequest = Setting;
                var success = await _BusinessSettings.AddSettingAsync(_BusinessSettings.CreateRequest);
                if (success == null)
                {
                    return CreateResponse<DTOSettings>(null!, StatusCodes.Status500InternalServerError, "Failed to add Setting.");
                }
                return CreateResponse<DTOSettings>(success, StatusCodes.Status201Created, "Order Added successfully");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOSettings>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("UpdateSetting", Name = "UpdateSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> UpdateSetting([FromBody] DTOSettingsURequest Setting)
        {
            try
            {
                if (Setting == null || Setting.ID <= 0)
                {
                    return CreateResponse<DTOSettings>(null!, StatusCodes.Status400BadRequest, "Invalid Setting data.");
                }
                _BusinessSettings.UpdateRequest = Setting;
                var success = await _BusinessSettings.UpdateSettingAsync(Setting);
                if (success == null)
                {
                    return CreateResponse<DTOSettings>(null!, StatusCodes.Status404NotFound, "Failed to update Setting.");
                }
                return CreateResponse<DTOSettings>(success, StatusCodes.Status200OK, "Setting updated successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOSettings>(null!, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("DeleteSetting/{ID}", Name = "DeleteSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEmployee([FromRoute] int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return CreateResponse<bool>(false, StatusCodes.Status400BadRequest, "ID <= 0.");
                }
                var success = await _BusinessSettings.DeleteSettingAsync(ID);
                if (!success)
                {
                    return CreateResponse<bool>(false, StatusCodes.Status404NotFound, "Failed to delete Setting.");
                }
                return CreateResponse<bool>(true, StatusCodes.Status200OK, "Setting deleted successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<bool>(false, StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}
