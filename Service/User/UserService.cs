using System.Net;
using System.Security.Claims;
using Dtos;
using Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using Models;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionGroupRepository _permissionGroupRepository;
        private readonly IApiExceptionService _apiExceptionService;

        public UserService(ILogger<UserService> logger,
            ITokenService tokenService,
            IUserRepository userRepository,
            IPermissionGroupRepository permissionGroupRepository,
            IApiExceptionService apiExceptionService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _permissionGroupRepository = permissionGroupRepository;
            _apiExceptionService = apiExceptionService;
        }

        public async Task<UserInfoDto> GetUser(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PermissionGroups = user.PermissionGroups.Select(x => x.ToDto()).ToList(),
                
            };
        }

        public async Task<UserDto> CreateUser(UserDto user)
        {
            var orgUser = user.Adapt<Models.User>();
            orgUser.Password = BCrypt.Net.BCrypt.HashPassword(orgUser.Password);
            var newUser = await _userRepository.CreateUser(orgUser);
            _logger.Log(LogLevel.Information, $"Created user with id {newUser.Id}");
            return newUser.Adapt<UserDto>();
        }
        
        public async Task<UserEditDto> UpdateUser(UserEditDto user)
        {
            var orgUser = user.Adapt<Models.User>();
            orgUser.SetUpdated();
            var newUser = await _userRepository.UpdateUser(orgUser);
            _logger.Log(LogLevel.Information, $"Updated user with id {newUser.Id}");
            return newUser.Adapt<UserEditDto>();
        }

        public async Task<bool> AddPermissionGroups(Guid userId, List<Guid> permissionGroupIds)
        {
            var orgUser = await _userRepository.GetUserById(userId);
            orgUser.PermissionGroups = await _permissionGroupRepository.GetPermissionGroupByIds(permissionGroupIds);
            orgUser.SetUpdated();
            var newUser = await _userRepository.UpdateUser(orgUser);
            _logger.Log(LogLevel.Information, $"Added permission group(s) to user with id {newUser.Id}");
            return true;
        }

        public async Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequest)
        {
            var user = await GetUserByUsername(authenticateRequest.Username);
            var validUser = CheckCredentials(user, authenticateRequest.Password);
            if (!validUser)
                throw _apiExceptionService.Create(HttpStatusCode.Unauthorized, Enums.MessageText.Unautorized.GetDescription());

            List<Claim> claims = new List<Claim>
            {
                new("username", user.Username),
                new("firstName", user.FirstName),
            };
            var roles = new List<int> {1, 2, 3, 4, 5, 6, 7, 8};
            claims.AddRange(roles.Select(x => new Claim("scopes", x.ToString())));

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = Guid.NewGuid();

            await _tokenService.SaveRefreshToken(authenticateRequest.ClientId, refreshToken, authenticateRequest.Username);
            
            return new AuthResponse {AccesToken = accessToken, RefreshToken = refreshToken};
        }

        private async Task<Models.User> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }

        private static bool CheckCredentials(Models.User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }

    public interface IUserService
    {
        Task<UserDto> CreateUser(UserDto user);
        Task<UserEditDto> UpdateUser(UserEditDto user);
        Task<bool> AddPermissionGroups(Guid userId, List<Guid> permissionGroupIds);
        Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequest);
    }
}