using System.Collections.Generic;
using System.Threading.Tasks;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.Core.Calendar.Services
{
    public interface IUserService
    {
        Task<bool> Insert(User user);
        Task<User> FindUser(string username, string password);
        Task<List<User>> Find();      
    }
}