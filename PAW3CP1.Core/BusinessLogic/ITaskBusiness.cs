using PAW3CP1.Data.Models;
using PAW3CP1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW3CP1.Core.BusinessLogic
{
    public interface ITaskBusiness
    {
        Task<IEnumerable<TaskDTO>> GetAllAsync();
        Task<IEnumerable<Tasks>> GetTask(int? id);
        Task<bool> SaveTaskAsync(Tasks tasks);
        Task<bool> DeleteTaskAsync(int id);
        Task<string> UpdateApprovalStatusAsync(int taskId, string newStatus, string role);

        
    }

}
