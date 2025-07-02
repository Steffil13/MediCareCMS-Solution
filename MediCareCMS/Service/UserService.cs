using MediCareCMS.Models;
using MediCareCMS.Repository;

namespace MediCareCMS.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string username, string password)
        {
            if (username == password)
                throw new Exception("Password cannot be same as username.");

            return _userRepository.ValidateUser(username, password);
        }
    }
}
