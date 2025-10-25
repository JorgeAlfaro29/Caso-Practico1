using PAW3CP1.Data.Repositories;
using PAW3CP1.Data.Models;

namespace PAW3CP1.Core.BusinessLogic
{
    public interface IUserBusiness
    {       
        Task<User?> ValidateLogin(string email);
    }
    public class UserBusiness(IRepositoryUser repositoryUser): IUserBusiness
    {
        /// </inheritdoc>
        public async Task<User?> ValidateLogin(string email)
        {
            return await repositoryUser.GetUserByEmailAsync(email);
        }
    }
}
