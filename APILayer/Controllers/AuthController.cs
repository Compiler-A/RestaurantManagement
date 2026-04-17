using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTOs.Employees;
using ContractsLayerRestaurant.DTOs.Login;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APILayer.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private ILoginService _Login;
        public AuthController(ILoginService Login)
        {
            _Login = Login;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTokenResponse>>> Login([FromBody] DTOLoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }

            var Data = await _Login.LoginAsync(request);
            return CreateResponse(Data, StatusCodes.Status200OK, "Login Successfully!");
        }


        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<DTOTokenResponse>>> Refresh([FromBody] DTORefreshRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var Data = await _Login.RefrehTokenAsync(request);
            return CreateResponse(Data, StatusCodes.Status200OK, "Refresh Successfully!");
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> Logout([FromBody] DTOLogoutRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request is null!");
            }
            var Data = await _Login.LogoutAsync(request);
            return CreateResponse(Data, StatusCodes.Status200OK, "Logout Successfully!");
        }

    }
}
