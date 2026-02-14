using BusinessLayerRestaurant;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/APISettings")]
    [ApiController]
    public class APISettings : BaseController
    {
        private readonly IBusinessSettings businessSettings;
       
        public APISettings(IBusinessSettings s)
        {
            businessSettings = s;
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
                    return CreateResponse<IEnumerable<DTOSettings>>(null!, 400, "Page number must be greater than 0.");
                }
                var list = await businessSettings.GetAllSettingsAsync(page);
                if (list == null || list.Count == 0)
                {
                    return CreateResponse<IEnumerable<DTOSettings>>(null!, 404, "Not Found!");
                }
                return CreateResponse<IEnumerable<DTOSettings>>(list, 200, $"Row: {list.Count}");
            }
            catch (Exception ex)
            {
                return CreateResponse<IEnumerable<DTOSettings>>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<DTOSettings>(null!, 400, "ID <= 0.");
                }
                var DTO = await businessSettings.GetSettingAsync(ID);
                if (DTO == null)
                {
                    return CreateResponse<DTOSettings>(null!, 404, "Not Found!");
                }
                return CreateResponse<DTOSettings>(DTO, 200, "Success");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOSettings>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("AddSetting", Name = "AddSetting")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> AddSetting([FromBody] DTOSettings Setting)
        {
            try
            {
                if (Setting == null || Setting.ID < 0)
                {
                    return CreateResponse<DTOSettings>(null!, 400, "Employee data is null.");
                }
                businessSettings.DTOSetting = Setting;
                var success = await businessSettings.Save();
                if (!success)
                {
                    return CreateResponse<DTOSettings>(null!, 500, "Failed to add Setting.");
                }
                return CreatedAtRoute(
                    "GetSetting",
                    new { ID = this.businessSettings.DTOSetting!.ID },
                    businessSettings.DTOSetting
                );
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOSettings>(null!, 500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("UpdateSetting", Name = "UpdateSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOSettings>>> UpdateSetting([FromBody] DTOSettings Setting)
        {
            try
            {
                if (Setting == null || Setting.ID <= 0)
                {
                    return CreateResponse<DTOSettings>(null!, 400, "Invalid Setting data.");
                }
                businessSettings.DTOSetting = await businessSettings.GetSettingAsync(Setting.ID);
                businessSettings.DTOSetting = Setting;
                var success = await businessSettings.Save();
                if (!success)
                {
                    return CreateResponse<DTOSettings>(null!, 404, "Failed to update employee.");
                }
                return CreateResponse<DTOSettings>(businessSettings.DTOSetting, 200, "Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<DTOSettings>(null!, 500, "Internal server error: " + ex.Message);
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
                    return CreateResponse<bool>(false, 400, "ID <= 0.");
                }
                var success = await businessSettings.Delete(ID);
                if (!success)
                {
                    return CreateResponse<bool>(false, 404, "Failed to delete employee.");
                }
                return CreateResponse<bool>(true, 200, "Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return CreateResponse<bool>(false, 500, "Internal server error: " + ex.Message);
            }
        }
    }
}
