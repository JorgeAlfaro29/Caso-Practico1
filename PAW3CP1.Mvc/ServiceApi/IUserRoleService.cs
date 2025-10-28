using System.Text.Json;
using PAW3CP1.Architecture;
using PAW3CP1.Architecture.Providers;
using PAW3CP1.Models.DTO;

namespace PAW3CP1.Mvc.ServiceApi
{
    public interface IUserRoleService
    {
        Task<List<UserRoleViewDTO>> GetUserRolesViewAsync();
        Task<bool> AssignRoleAsync(int userId, int roleId);
    }

    public class UserRoleService(IRestProvider restProvider) : IUserRoleService
    {
        private readonly string _baseUrl = "https://localhost:7265/userroles";

        public async Task<List<UserRoleViewDTO>> GetUserRolesViewAsync()
        {
            var response = await restProvider.GetAsync($"{_baseUrl}/view", null);
            return await JsonProvider.DeserializeAsync<List<UserRoleViewDTO>>(response);
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            var payload = JsonSerializer.Serialize(new { userId, roleId });
            var response = await restProvider.PostAsync($"{_baseUrl}/assign", payload);

            return response.Contains("success", StringComparison.OrdinalIgnoreCase);
        }

    }

}
