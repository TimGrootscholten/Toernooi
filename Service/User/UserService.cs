using System.Net;
using System.Security.Claims;
using Dtos;
using Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using Models;

namespace Services.User;

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

    public async Task<UserInfoDto> GetUserById(Guid id)
    {
        var user = await _userRepository.GetUserById(id);
        return user.ToInfoDto();
    }

    public async Task<List<UserInfoDto>> GetUsers()
    {
        var users = await _userRepository.GetUsers();
        return users.Adapt<List<UserInfoDto>>();
    }

    public async Task<AuthResponse> CreateUser(UserDto user)
    {
        var orgUser = user.Adapt<Models.User>();
        var isUsernameUnique = await _userRepository.IsUsernameUnique(orgUser.Username);
        if (isUsernameUnique) throw _apiExceptionService.Create(HttpStatusCode.BadRequest, "Gebruikersnaam bestaat al");
        orgUser.Password = BCrypt.Net.BCrypt.HashPassword(orgUser.Password);
        var everyonePermissionGroup = await _permissionGroupRepository.GetPermissionGroupById(Guid.Parse("9cc607c1-7b93-4245-98f5-0d788cf94895"));
        orgUser.PermissionGroups = new List<PermissionGroup> {everyonePermissionGroup};
        var newUser = await _userRepository.CreateUser(orgUser);
        _logger.Log(LogLevel.Information, $"Created user with id {newUser.Id}");
        return await Authenticate(new AuthenticateRequestDto {ClientId = user.ClientId, Username = user.Username, Password = user.Password});
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
        orgUser.PermissionGroups.AddRange(await _permissionGroupRepository.GetPermissionGroupByIds(permissionGroupIds));
        orgUser.SetUpdated();
        var newUser = await _userRepository.UpdateUser(orgUser);
        _logger.Log(LogLevel.Information, $"Added permission group(s) to user with id {newUser.Id}");
        return true;
    }

    public async Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequest)
    {
        var user = await _userRepository.GetUserByUsername(authenticateRequest.Username);
        var validUser = CheckCredentials(user, authenticateRequest.Password);
        if (!validUser) throw _apiExceptionService.Create(HttpStatusCode.Unauthorized, "Gebruikersnaam of wachtwoord is niet juist");
        var claims = new List<Claim>
        {
            new("username", user.Username),
            new("name", user.FirstName + user.LastName)
        };
        var newClaims = await AddPermissionsToClaims(user, claims);

        var accessToken = _tokenService.GenerateAccessToken(newClaims);
        var refreshToken = Guid.NewGuid();

        await _tokenService.SaveRefreshToken(authenticateRequest.ClientId, refreshToken, authenticateRequest.Username);

        return new AuthResponse {AccessToken = accessToken, RefreshToken = refreshToken};
    }

    public async Task<AuthResponse> AuthenticateWithRefreshToken(AuthenticateWithRefreshTokenDto authenticateWithRefreshToken)
    {
        var refreshToken = await _tokenService.CheckRefreshToken(authenticateWithRefreshToken.ClientId, authenticateWithRefreshToken.RefreshToken);
        var user = await _userRepository.GetUserByUsername(refreshToken.Username);

        var claims = new List<Claim>
        {
            new("username", user.Username),
            new("name", user.FirstName + user.LastName)
        };
        var newClaims = await AddPermissionsToClaims(user, claims);

        var accessToken = _tokenService.GenerateAccessToken(newClaims);
        var newRefreshToken = Guid.NewGuid();

        await _tokenService.SaveRefreshToken(authenticateWithRefreshToken.ClientId, newRefreshToken, user.Username);
        return new AuthResponse {AccessToken = accessToken, RefreshToken = newRefreshToken};
    }

    public async Task<bool> DeleteClientGrant(Guid clientId)
    {
        return await _tokenService.DeleteClientGrant(clientId);
    }

    public async Task<bool> IsUniqueUsername(string username)
    {
        return !await _userRepository.IsUsernameUnique(username);
    }

    private static bool CheckCredentials(Models.User user, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    private async Task<List<Claim>> AddPermissionsToClaims(Models.User user, List<Claim> claims)
    {
        var permissions = user.PermissionGroups != null
            ? await _permissionGroupRepository.GetPermissionsByPermissionGroupByIds(user.PermissionGroups.Select(x => x.Id).ToList())
            : new List<int>();
        claims.AddRange(permissions.Select(x => new Claim("scopes", x.ToString())));
        return claims;
    }
}

public interface IUserService
{
    Task<UserInfoDto> GetUserById(Guid id);
    Task<List<UserInfoDto>> GetUsers();
    Task<AuthResponse> CreateUser(UserDto user);
    Task<UserEditDto> UpdateUser(UserEditDto user);
    Task<bool> AddPermissionGroups(Guid userId, List<Guid> permissionGroupIds);
    Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequest);
    Task<AuthResponse> AuthenticateWithRefreshToken(AuthenticateWithRefreshTokenDto authenticateWithRefreshToken);
    Task<bool> DeleteClientGrant(Guid clientId);
    Task<bool> IsUniqueUsername(string username);
    
}