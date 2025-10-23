using System;
using System.Collections.Generic;
using System.Linq;
using PAW3CP1.Data.Models;
using PAW3CP1.Models.DTO;

namespace PAW3CP1.Data.Extensions
{
    public static class TaskDTOExtensions
    {
        public static Tasks ToTasks(TaskDTO dto)
        {
            return new Tasks
            {
                Id = dto.Id ?? 0,
                Name = dto.Name ?? string.Empty,
                Description = dto.Description,
                DueDate = dto.DueDate ?? DateTime.MinValue,
                CreatedAt = dto.CreatedAt,
                Status = dto.Status ?? string.Empty,
                Approved = dto.Approved
            };
        }
    }
}
