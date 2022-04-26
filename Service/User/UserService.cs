using System.Net;
using System.Security.Claims;
using Dtos;
using Repositories;
using Mapster;
using Models;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IApiExceptionService _apiExceptionService;

        public UserService(IUserRepository userRepository,
            ITokenService tokenService,
            IApiExceptionService apiExceptionService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _apiExceptionService = apiExceptionService;
        }

        public UserDto CreateUser(UserDto user)
        {
            var orgUser = user.Adapt<Models.User>();
            orgUser.Password = BCrypt.Net.BCrypt.HashPassword(orgUser.Password);
            return _userRepository.CreateUser(orgUser).Adapt<UserDto>();
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
            var roles = new List<int> {1, 2};
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
        UserDto CreateUser(UserDto user);
        Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequest);
    }
}