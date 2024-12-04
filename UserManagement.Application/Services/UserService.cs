using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;

namespace UserManagement.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }
    }
}
