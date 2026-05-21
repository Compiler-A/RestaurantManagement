#pragma warning disable CA1416 // Validate platform compatibility
using ContractsLayerRestaurant.Interfaces.Services;
using System.Security.Authentication;
using ContractsLayerRestaurant.DTORequest.Auth;
using ContractsLayerRestaurant.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using DomainLayer.Entities;
using ContractsLayerRestaurant.DTOResponse;
using ContractsLayerRestaurant.Configuration;

namespace BusinessLayerRestaurant.Operations
{

    public class LoginContainer : ILoginServiceContainer
    {
        private IEmployeesService _IEmployees;
        public IEmployeesService IEmployee
        {
            get => _IEmployees;
            set => _IEmployees = value;
        }
        private ILoginRepository _ILoginRepository;
        public ILoginRepository IData
        {
            get => _ILoginRepository;
            set => _ILoginRepository = value;
        }

        public LoginContainer(IEmployeesService Employee, ILoginRepository loginRepository)
        {
            _IEmployees = Employee;
            _ILoginRepository = loginRepository;
        }
    }


    public class LoginReader : ILoginServiceReader
    {
        private ILoginServiceContainer _Interface;
        private readonly IMyLogger _Logger;

        public LoginReader(ILoginServiceContainer Interface, IMyLogger Logger)
        {
            _Interface = Interface;
            _Logger = Logger;
        }

        public async Task<Auth?> GetAsync(string UserName)
        {
            var Data = await _Interface.IData.GetDataAsync(UserName);
            if (Data == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Token Found, UserName: {UserName}", EventLogEntryType.Information);
            return Data;
        }
    }

    public class LoginWriter : ILoginServiceWriter
    {
        private ILoginServiceContainer _Interface;
        private ILoginServiceReader _Reader;
        private IOptions<JwtSettings> _Options;

        public LoginWriter(IOptions<JwtSettings> Options,ILoginServiceContainer Interface,ILoginServiceReader Reader)
        {
            _Interface = Interface;
            _Reader = Reader;
            _Options = Options;
        }
        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        private string _GetAccessToken(int ID, string UserName, string Role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, ID.ToString()),
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.Role, Role)
            };

            
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_Options.Value.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _Options.Value.Issuer,
                audience: _Options.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_Options.Value.ExpirationMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<DTOTokenResponse> LoginAsync(DTOLoginRequest Request)
        {
            var auth = await _Interface.IData.LoginAsync(Request.UserName);

            if (auth == null || auth.Employee == null)
                throw new AuthenticationException("Invalid credentials");

            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(Request.Password, auth.Employee.Password);

            if (!isValidPassword)
                throw new AuthenticationException("Invalid credentials");

            if (auth.Employee.JobRole == null)
            {
                throw new InvalidOperationException("Not Found Job Role");
            }

            var accessToken = _GetAccessToken(auth.Employee.EmployeeID, auth.Employee.Username, auth.Employee.JobRole.JobName);
            var refreshToken = GenerateRefreshToken();

            var request = new DTOAuthCURequest
            {
                EmployeeID = auth.Employee.EmployeeID,
                RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
            };


            var result = await _Interface.IData.CreateDataAsync(request);
            if (!result)
            {
                throw new InvalidOperationException("Faild Login");
            }
            return new DTOTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<DTOTokenResponse> RefrehTokenAsync(DTORefreshRequest Request)
        {
            var auth = await _Reader.GetAsync(Request.UserName);

            if (auth == null)
                throw new AuthenticationException("Invalid refresh request");

            if (auth.RefreshTokenRevokedAt != null)
                throw new AuthenticationException("Refresh token is revoked");

            if (auth.RefreshTokenExpiresAt == null || auth.RefreshTokenExpiresAt <= DateTime.UtcNow)
                throw new AuthenticationException("Refresh token expired");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(Request.RefreshToken, auth.RefreshTokenHash);
            if (!refreshValid)
                throw new AuthenticationException("Invalid refresh token");

            if (auth.Employee == null || auth.Employee.JobRole ==null)
            {
                throw new InvalidOperationException("Not Found Job Role or Employee");
            }

            var newAccessToken = _GetAccessToken(auth.Employee.EmployeeID, auth.Employee.Username, auth.Employee.JobRole.JobName);

            // Rotation: replace refresh token
            var newRefreshToken = GenerateRefreshToken();
            var request = new DTOAuthCURequest
            {
                EmployeeID = auth.Employee.EmployeeID,
                RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            var result = await _Interface.IData.UpdateDataAsync(request);
            if (!result)
            {
                throw new InvalidOperationException("Faild Refresh token");
            }

            return new DTOTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> LogoutAsync(DTOLogoutRequest Request)
        {
            var employee = await _Interface.IData.GetDataAsync(Request.UserName);

            if (employee == null)
                return true;

            bool refreshValid = BCrypt.Net.BCrypt.Verify(Request.RefreshToken, employee.RefreshTokenHash);
            if (!refreshValid)
                return true;
            if (!await _Interface.IData.LogoutDataAsync(employee.EmployeeID))
            {
                throw new InvalidOperationException("Faild Logout!");
            }
            return true;
        }
    }
}
