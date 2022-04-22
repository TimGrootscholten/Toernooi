using System.Net;
using System.Security.Claims;
using Repositories;
using Mapster;
using Models;
using Dtos;

namespace Services
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
            User orgUser = user.Adapt<User>();
            orgUser.Password = BCrypt.Net.BCrypt.HashPassword(orgUser.Password);
            return _userRepository.CreateUser(orgUser).Adapt<UserDto>();
        }

        public async Task<AuthResponse> Authenticate(AuthenticateRequestDto authenticateRequest)
        {
            User user = await GetUserByUsername(authenticateRequest.Email);
            bool validUser = CheckCredentials(user, authenticateRequest.Password);
            if (!validUser) throw _apiExceptionService.Create(HttpStatusCode.Unauthorized, Enums.MessageText.Unautorized.GetDescription());
            
            List<Claim> claims = new List<Claim>
            {
                new("username", user.Username),
                new("firstName", user.FirstName)
            };

            string accessToken = _tokenService.GenerateAccessToken(claims);
            Guid refreshToken = Guid.NewGuid();

            return new AuthResponse {AccesToken = accessToken, RefreshToken = refreshToken};
        }

        private async Task<User> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }

        private bool CheckCredentials(User user, string password)
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
