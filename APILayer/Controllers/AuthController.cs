using APILayer.Filters;
using BusinessLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;


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
        [EnableRateLimiting(NameRateLimitPolicies.Auth)]
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
        [EnableRateLimiting(NameRateLimitPolicies.Auth)]
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
