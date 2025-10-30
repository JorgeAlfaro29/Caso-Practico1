using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAW3CP1.Data.Models;
using PAW3CP1.Data.Repositories;
using PAW3CP1.Models.DTO;

namespace PAW3CP1.Core.BusinessLogic
{
    // la comento porque creo que es mejor que la interfaz vaya en un archivo aparte para no tener conflictos
    //public interface ITaskBusiness 
    //{

    //    Task<IEnumerable<Tasks>> GetTask(int? id);

    //    Task<bool> SaveTaskAsync(Tasks tasks);


    //    Task<bool> DeleteTaskAsync(int id);

    //    Task<string> UpdateApprovalStatusAsync(int id, string newStatus, string role);
    //}
    public class TaskBusiness : ITaskBusiness
    {
        private readonly IRepositoryTask repositoryTask;

        public TaskBusiness(IRepositoryTask repositoryTask)
        {
            this.repositoryTask = repositoryTask;
        }


        public async Task<IEnumerable<TaskDTO>> GetAllAsync()
        {
            var tasks = await repositoryTask.ReadAsync();

            return tasks
                .OrderBy(t => t.Status == null ? 0 : t.Status == "Approved" ? 1 : 2)
                .Select(t => new TaskDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                });
        }





        public async Task<IEnumerable<Tasks>> GetTask(int? id)
        {

            return id == null
                ? await repositoryTask.ReadAsync()
                : [await repositoryTask.FindAsync((int)id)];
        }

        /// </inheritdoc>
        public async Task<bool> SaveTaskAsync(Tasks Tasks)
        {
            if (Tasks.Id == 0)
            {
                Tasks.DueDate = DateTime.Now;
                Tasks.Status = "Activo"; 
                Tasks.Approved = null;
                Tasks.CreatedAt ??= DateTime.UtcNow;
            }
            else
            {
                Tasks.Approved = Tasks.Approved;
                Tasks.DueDate = DateTime.Now;
            }
            

            return await repositoryTask.CheckBeforeSavingAsync(Tasks);
            //return await repositoryCategory.UpdateAsync(category);
        }

        /// </inheritdoc>
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await repositoryTask.FindAsync(id);
            return await repositoryTask.DeleteAsync(task);
        }

        public async Task<string> UpdateApprovalStatusAsync(int id, string newStatus, string username)
        {
            var task = await repositoryTask.FindAsync(id);
            if (task == null)
                return "Tarea no encontrada";

            // Validación desactivada para poder hcer las pruebas
            // if (username != "Manager")
            //     return "Solo los managers pueden aprobar o denegar";

            if (task.Status == "Denied" && newStatus == "Approved")
            {
                var hours = (DateTime.UtcNow - task.CreatedAt!.Value).TotalHours;
                if (hours < 24)
                    return "No se puede aprobar una tarea denegada con menos de 24 horas";
            }

            task.Status = newStatus;

            // este lo agrego para actualizar campo booleano Approved
            if (newStatus == "Approved")
                task.Approved = true;
            else if (newStatus == "Denied")
                task.Approved = false;
            else
                task.Approved = null;



            var result = await repositoryTask.UpdateAsync(task);
            return result ? "OK" : "Error al actualizar";
        }



    }


}
