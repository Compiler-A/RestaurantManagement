#pragma warning disable CA1416 // Validate platform compatibility
using BusinessLayerRestaurant.Interfaces;
using System.Security.Authentication;
using ContractsLayerRestaurant.DTORequest.Auth;
using DataLayerRestaurant.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using DomainLayer.Entities;
using ContractsLayerRestaurant.DTOResponse;


namespace BusinessLayerRestaurant.Classes
{

    public class clsLoginContainer : ILoginServiceContainer
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

        public clsLoginContainer(IEmployeesService Employee, ILoginRepository loginRepository)
        {
            _IEmployees = Employee;
            _ILoginRepository = loginRepository;
        }
    }

    public class clsEmployeeLoaderByLogin : ILoginServiceComposition
    {
        private IEmployeesService _IData;

        public clsEmployeeLoaderByLogin(IEmployeesService Employee)
        {
            _IData = Employee;
        }

        public async Task LoadDataAsync(Auth item)
        {
            item.Employees = await _IData.GetEmployeeAsync(item.EmployeeID);
        }

    }

    public class clsCompositionLoginLoader : ILoginServiceComposition
    {
        private IEnumerable<ILoginServiceComposition> _loaders;
        public clsCompositionLoginLoader
            (IEnumerable<ILoginServiceComposition> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(Auth item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsLoginReader : clsCompositionLoginLoader, ILoginServiceReader
    {
        private ILoginServiceContainer _Interface;
        private readonly IMyLogger _Logger;

        public clsLoginReader(ILoginServiceContainer Interface, IMyLogger Logger, IEnumerable<ILoginServiceComposition> loaders)
            : base(loaders)
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
            await LoadDataAsync(Data);
            _Logger.EventLogs($"Token Found, UserName: {UserName}", EventLogEntryType.Information);
            return Data;
        }
    }

    public class clsLoginWriter : clsCompositionLoginLoader , ILoginServiceWriter
    {
        private ILoginServiceContainer _Interface;
        private ILoginServiceReader _Reader;
        private IOptions<JwtSettings> _Options;

        public clsLoginWriter(IOptions<JwtSettings> Options,ILoginServiceContainer Interface,ILoginServiceReader Reader, IEnumerable<ILoginServiceComposition> loaders)
            : base(loaders)
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
            var employee = await _Interface.IEmployee.GetEmployeeAsync(Request.UserName);

            if (employee == null)
                throw new AuthenticationException("Invalid credentials");

            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(Request.Password, employee.PasswordHashed);

            if (!isValidPassword)
                throw new AuthenticationException("Invalid credentials");

            if (employee.JobRoles == null)
            {
                throw new InvalidOperationException("Not Found Job Role");
            }

            var accessToken = _GetAccessToken(employee.ID, employee.UserName, employee.JobRoles.Name);
            var refreshToken = GenerateRefreshToken();

            var request = new DTOAuthCURequest
            {
                EmployeeID = employee.ID,
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
            var employee = await _Reader.GetAsync(Request.UserName);

            if (employee == null)
                throw new AuthenticationException("Invalid refresh request");

            if (employee.RefreshTokenRevokedAt != null)
                throw new AuthenticationException("Refresh token is revoked");

            if (employee.RefreshTokenExpiresAt == null || employee.RefreshTokenExpiresAt <= DateTime.UtcNow)
                throw new AuthenticationException("Refresh token expired");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(Request.RefreshToken, employee.RefreshTokenHash);
            if (!refreshValid)
                throw new AuthenticationException("Invalid refresh token");

            if (employee.Employees == null || employee.Employees.JobRoles ==null)
            {
                throw new InvalidOperationException("Not Found Job Role or Employee");
            }

            var newAccessToken = _GetAccessToken(employee.EmployeeID, employee.Employees.UserName, employee.Employees.JobRoles.Name);

            // Rotation: replace refresh token
            var newRefreshToken = GenerateRefreshToken();
            var request = new DTOAuthCURequest
            {
                EmployeeID = employee.EmployeeID,
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


    public class clsLoginService : ILoginService
    {
        private ILoginServiceReader _Reader;
        private ILoginServiceWriter _Writer;
        private ILoginServiceContainer _Interface;

        public clsLoginService(ILoginServiceContainer Interface, ILoginServiceReader Reader, ILoginServiceWriter Writer)
        {
            _Interface = Interface;
            _Reader = Reader;
            _Writer = Writer;
        }

        public IEmployeesService IEmployee
        {
            get => _Interface.IEmployee;
            set => _Interface.IEmployee = value;
        }

        public async Task<Auth?> GetAsync(string UserName)
        {
            return await _Reader.GetAsync(UserName);
        }
        public async Task<DTOTokenResponse> LoginAsync(DTOLoginRequest Request)
        {
            return await _Writer.LoginAsync(Request);
        }
        public async Task<DTOTokenResponse> RefrehTokenAsync(DTORefreshRequest Request)
        {
            return await _Writer.RefrehTokenAsync(Request);
        }

        public async Task<bool> LogoutAsync(DTOLogoutRequest Request)
        {
            return await _Writer.LogoutAsync(Request);
        }

    }
}
