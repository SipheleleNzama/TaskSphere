using AutoMapper;
using TaskSphere.Application.DTOs;
using TaskSphere.Domain.Entities;
using TaskSphere.Domain.Enums;
using TaskSphere.Domain.Interfaces;
using TaskStatus = TaskSphere.Domain.Enums.TaskStatus;

namespace TaskSphere.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskDto>> GetAllByUserIdAsync(int userId)
    {
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<IEnumerable<TaskDto>> GetByStatusAsync(int userId, TaskStatus status)
    {
        var tasks = await _taskRepository.GetByStatusAsync(userId, status);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<TaskDto?> GetByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
            return null;

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateAsync(int userId, CreateTaskDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        task.UserId = userId;
        task.Status = TaskStatus.ToDo;

        var created = await _taskRepository.CreateAsync(task);
        return _mapper.Map<TaskDto>(created);
    }

    public async Task<TaskDto?> UpdateAsync(int id, int userId, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
            return null;

        _mapper.Map(dto, task);
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskDto>(task);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
            return false;

        await _taskRepository.DeleteAsync(id);
        return true;
    }
}