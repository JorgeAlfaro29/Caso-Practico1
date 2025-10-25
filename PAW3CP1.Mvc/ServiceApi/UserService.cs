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
    public class UserService (IRestProvider restProvider) : IUserService
    {
        private readonly string _baseUrlMinimalApi = "https://localhost:7265/login";


        public async Task<UserDTO?> CheckLogin(string email)
        {
            try
            {
                var url = $"{_baseUrlMinimalApi}?email={Uri.EscapeDataString(email)}";
                var response = await restProvider.GetAsync(url, null);
                return await JsonProvider.DeserializeAsync<UserDTO>(response);
            }
            catch (InvalidOperationException)
            {
                // Usuario no encontrado
                return null;
            }
        }
    }


}
