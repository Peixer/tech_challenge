using System.Collections.Generic;
using System.Threading.Tasks;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.Core.Calendar.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Insert(User user);
        Task<List<User>> Find();        
        Task<User> FindUser(string username, string password);
        Task<User> FindUserByUsername(string username);
        Task<User> FindUserById(string id);
    }
}