using System.Drawing;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using PAW3CP1.Architecture;
using PAW3CP1.Architecture.Providers;
using PAW3CP1.Models.DTO;

namespace PAW3CP1.Mvc.ServiceApi
{
    public interface IUserService
    {
        Task<UserDTO?> CheckLogin(string email);
    }

    public class UserService : IUserService
    {
        private readonly IRestProvider _restProvider;
        private readonly string _baseUrlMinimalApi = "https://localhost:7265/login";

        public UserService(IRestProvider restProvider)
        {
            _restProvider = restProvider;
        }

        public async Task<UserDTO?> CheckLogin(string email)
        {
            try
            {
                var url = $"{_baseUrlMinimalApi}?email={Uri.EscapeDataString(email)}";
                var response = await _restProvider.GetAsync(url, null);

                if (string.IsNullOrWhiteSpace(response))
                    return null;

                // Intentar deserializar con el JsonProvider si lo tienes; si falla, fallback a System.Text.Json
                try
                {
                    return await JsonProvider.DeserializeAsync<UserDTO>(response);
                }
                catch
                {
                    return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (InvalidOperationException)
            {
                // Usuario no encontrado u otro problema controlado
                return null;
            }
            catch (Exception)
            {
                // Para depuración puedes loggear el error; por ahora devolvemos null para que la app no rompa.
                return null;
            }
        }
    }


}
