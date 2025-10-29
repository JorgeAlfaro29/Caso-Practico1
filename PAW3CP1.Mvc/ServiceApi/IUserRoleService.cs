using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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
        Task<List<RoleDTO>> GetAllRolesAsync(TaskDbContext taskDbContext);

    }

    public class UserRoleService : IUserRoleService
    {
        private readonly IRestProvider _restProvider;
        private readonly string _baseUrl = "https://localhost:7265/userroles";

        public UserRoleService(IRestProvider restProvider)
        {
            _restProvider = restProvider;
        }

        public async Task<List<UserRoleDTO>> GetUserRolesViewAsync()
        {
            try
            {
                var response = await _restProvider.GetAsync($"{_baseUrl}/view", null);

                if (string.IsNullOrWhiteSpace(response))
                    return new List<UserRoleDTO>();

                try
                {
                    return await JsonProvider.DeserializeAsync<List<UserRoleDTO>>(response);
                }
                catch
                {
                    return JsonSerializer.Deserialize<List<UserRoleDTO>>(response, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<UserRoleDTO>();
                }
            }
            catch (Exception)
            {
                // En caso de error devolvemos lista vacía; para depuración puedes loggear la excepción
                return new List<UserRoleDTO>();
            }
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            try
            {
                var payload = JsonSerializer.Serialize(new { userId, roleId });
                var response = await _restProvider.PostAsync($"{_baseUrl}/assign", payload);

                return !string.IsNullOrEmpty(response) && response.Contains("success", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                // Si falla la petición consideramos que no se asignó correctamente
                return false;
            }
        }

        public async Task<UserRoleDTO?> GetUserRoleByIdAsync(int userId)
        {
            try
            {
                var response = await _restProvider.GetAsync($"{_baseUrl}/view/{userId}", null);

                if (string.IsNullOrWhiteSpace(response))
                    return null;

                try
                {
                    return await JsonProvider.DeserializeAsync<UserRoleDTO?>(response);
                }
                catch
                {
                    return JsonSerializer.Deserialize<UserRoleDTO>(response, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<RoleDTO>> GetAllRolesAsync(TaskDbContext taskDbContext)
        {
            // Suponiendo que tienes acceso a tu DbContext, por ejemplo _context
            return await taskDbContext.Roles
                .Select(r => new RoleDTO
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Description = r.Description
                })
                .ToListAsync();
        }

    }

}
