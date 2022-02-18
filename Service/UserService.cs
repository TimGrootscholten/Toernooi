using Repositories;
using Mapster;
using Model;
using Dtos;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto CreateUser(UserDto user)
        {
            User orgUser = user.Adapt<User>();
            return _userRepository.CreateUser(orgUser).Adapt<UserDto>();
        }
    }

    public interface IUserService
    {
        UserDto CreateUser(UserDto user);
    }
}
