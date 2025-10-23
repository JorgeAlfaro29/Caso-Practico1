using System.Text.Json;
using PAW3CP1.Architecture;
using PAW3CP1.Architecture.Providers;
using PAW3CP1.Models.DTO;

namespace PAW3CP1.Mvc.ServiceApi
{
    public interface ITaskService
    {
        Task<IEnumerable<T>> GetDataAsync<T>();
        Task<TaskDTO?> CreateAsync(TaskDTO tasks);
        Task<bool> UpdateAsync(int id, TaskDTO tasks);
        Task<bool> DeleteAsync(int id);



    }
    public class TaskService(IRestProvider restProvider) : ITaskService
    {
        private readonly string _baseUrl = "https://localhost:7254/api/TasksApi";
        private readonly string _baseUrlMinimalApi = "https://localhost:7265/Task";


        public async Task<IEnumerable<T>> GetDataAsync<T>()
        {
            var response = await restProvider.GetAsync(_baseUrlMinimalApi, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<T>>(response);
        }

        public async Task<TaskDTO?> CreateAsync(TaskDTO tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            // Serializamos el objeto
            var body = JsonSerializer.Serialize(tasks);

            // POST a la API real de categorías
            var response = await restProvider.PostAsync(_baseUrl, body);

            // Deserializamos la respuesta
            return await JsonProvider.DeserializeAsync<TaskDTO>(response);
        }

        public async Task<bool> UpdateAsync(int id, TaskDTO tasks)
        {
            var url = $"{_baseUrl}/{id}";
            var body = JsonSerializer.Serialize(tasks);
            try
            {
                var response = await restProvider.PutAsync(url, string.Empty, body);
            }
            catch(Exception ex)
            {
                // Manejo de errores (puedes registrar el error o lanzar una excepción personalizada)
                Console.WriteLine($"Error al actualizar la tarea: {ex.Message}");
                return false;
            }
            return true; // !string.IsNullOrWhiteSpace(response);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = $"{_baseUrl}/{id}";
            var response = await restProvider.DeleteAsync(url, string.Empty);
            return !string.IsNullOrWhiteSpace(response);
        }
    }

    
}
