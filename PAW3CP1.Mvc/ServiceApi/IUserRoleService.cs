using System.Text.Json;
using PAW3CP1.Architecture;
using PAW3CP1.Architecture.Providers;
using PAW3CP1.Data.Models;
using PAW3CP1.Models.DTO;

namespace PAW3CP1.Mvc.ServiceApi
{
    public interface IUserRoleService
    {
        Task<List<UserRoleDTO>> GetUserRolesViewAsync();
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<UserRoleDTO?> GetUserRoleByIdAsync(int userId);

    }

    public class UserRoleService(IRestProvider restProvider) : IUserRoleService
    {
        private readonly string _baseUrl = "https://localhost:7265/userroles";

        public async Task<List<UserRoleDTO>> GetUserRolesViewAsync()
        {
            var response = await restProvider.GetAsync($"{_baseUrl}/view", null);
            return await JsonProvider.DeserializeAsync<List<UserRoleDTO>>(response);
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            var payload = JsonSerializer.Serialize(new { userId, roleId });
            var response = await restProvider.PostAsync($"{_baseUrl}/assign", payload);

            return response.Contains("success", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<UserRoleDTO?> GetUserRoleByIdAsync(int userId)
        {
            var response = await restProvider.GetAsync($"{_baseUrl}/view/{userId}", null);
            return await JsonProvider.DeserializeAsync<UserRoleDTO?>(response);
        }


    }

}
