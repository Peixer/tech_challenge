using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Core.Calendar.Data;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.Core.Calendar.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly APIContext _apiContext;

        public UserRepository(APIContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task<bool> Insert(User user)
        {
            this._apiContext.Users.Add(user);
            await this._apiContext.SaveChangesAsync();

            return true;
        }

        public Task<List<User>> Find()
        {
            return this._apiContext.Users.ToListAsync();
        }

        public async Task<User> FindUser(string username, string password)
        {
            return await this._apiContext.Users.SingleOrDefaultAsync(x =>
                string.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase) && x.Password == password);
        }

        public async Task<User> FindUserByUsername(string username)
        {
            return await this._apiContext.Users.SingleOrDefaultAsync(x => string.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}