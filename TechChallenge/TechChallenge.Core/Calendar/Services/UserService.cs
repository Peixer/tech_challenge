using System.Collections.Generic;
using System.Threading.Tasks;
using TechChallenge.Core.Calendar.Entities;
using TechChallenge.Core.Calendar.Repositories;

namespace TechChallenge.Core.Calendar.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<bool> Insert(User user)
        {
            var userWithSameUsername = await _userRepository.FindUserByUsername(user.Username);

            if (userWithSameUsername != null)
                return false;

            await _userRepository.Insert(user);
            return true;
        }

        public Task<User> FindUser(string username, string password)
        {
            return this._userRepository.FindUser(username, password);
        }
        public Task<List<User>> Find()
        {
            return this._userRepository.Find();
        }
    }
}