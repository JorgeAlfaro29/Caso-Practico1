using Microsoft.EntityFrameworkCore;
using PAW3CP1.Data.Models;

namespace PAW3CP1.Data.Repositories
{
    
    public interface IRepositoryUser
    {
        Task<IEnumerable<User>> ReadAsync();
        Task<User> GetUserByEmailAsync(string email);
    }

    public class RepositoryUser : RepositoryBase<User>, IRepositoryUser
    {
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user ?? throw new InvalidOperationException($"User with email '{email}' not found.");

            /* Non-simplified method:
            if (user == null)
                throw new InvalidOperationException($"User with email '{email}' not found.");
            return user;
            */
        }
    }
}
