using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSphere.Application.DTOs;
using TaskSphere.Domain.Enums;
using TaskStatus = TaskSphere.Domain.Enums.TaskStatus;

namespace TaskSphere.Application.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllByUserIdAsync(int userId);
        Task<IEnumerable<TaskDto>> GetByStatusAsync(int userId, TaskStatus status);
        Task<TaskDto?> GetByIdAsync(int id, int userId);
        Task<TaskDto> CreateAsync(int userId, CreateTaskDto dto);
        Task<TaskDto?> UpdateAsync(int id, int userId, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
