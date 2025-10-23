using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAW3CP1.Data.Models;
using PAW3CP1.Data.Repositories;

namespace PAW3CP1.Core.BusinessLogic
{
    public interface ITaskBusiness
    {
        
        Task<IEnumerable<Tasks>> GetTask(int? id);
        
        Task<bool> SaveTaskAsync(Tasks tasks);
        

        Task<bool> DeleteTaskAsync(int id);
    }
    public class TaskBusiness(IRepositoryTask repositoryTask): ITaskBusiness
    {

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
                Tasks.CreatedAt = DateTime.Now;
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

    }

   
}
