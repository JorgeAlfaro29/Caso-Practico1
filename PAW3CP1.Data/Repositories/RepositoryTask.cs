using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAW3CP1.Data.Models;

namespace PAW3CP1.Data.Repositories
{
    
    public interface IRepositoryTask
    {
        Task<bool> UpsertAsync(Tasks entity, bool isUpdating);
        Task<bool> CreateAsync(Tasks entity);
        Task<bool> DeleteAsync(Tasks entity);
        Task<IEnumerable<Tasks>> ReadAsync();
        Task<Tasks> FindAsync(int id);
        Task<bool> UpdateAsync(Tasks entity);
        Task<bool> UpdateManyAsync(IEnumerable<Tasks> entities);
        Task<bool> ExistsAsync(Tasks entity);
        Task<bool> CheckBeforeSavingAsync(Tasks entity);
    }

    public class RepositoryTask : RepositoryBase<Tasks>, IRepositoryTask
    {
        // verifica si existe
        public async new Task<bool> ExistsAsync(Tasks entity)
        {
            return await DbContext.Tasks.AnyAsync(x => x.Id == entity.Id);
        }

        // verifica si existe y si existe actualiza, sino crea
        public async Task<bool> CheckBeforeSavingAsync(Tasks entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists)
            {
                // algo mas 
                
            }

            return await UpsertAsync(entity, exists);

        }

    
    }
    
    
}
