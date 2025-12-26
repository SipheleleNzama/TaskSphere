using Microsoft.AspNetCore.Mvc;
using TaskSphere.Application.DTOs;
using TaskSphere.Application.Services;
using TaskSphere.Domain.Enums;
using TaskStatus = TaskSphere.Domain.Enums.TaskStatus;

namespace TaskSphere.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetUserTasks(int userId)
        {
            var tasks = await _taskService.GetAllByUserIdAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("user/{userId}/status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByStatus(int userId, TaskStatus status)
        {
            var tasks = await _taskService.GetByStatusAsync(userId, status);
            return Ok(tasks);
        }

        [HttpGet("{id}/user/{userId}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id, int userId)
        {
            var task = await _taskService.GetByIdAsync(id, userId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost("user/{userId}")]
        public async Task<ActionResult<TaskDto>> CreateTask(int userId, [FromBody] CreateTaskDto dto)
        {
            var task = await _taskService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id, userId = task.UserId }, task);
        }

        [HttpPut("{id}/user/{userId}")]
        public async Task<ActionResult<TaskDto>> UpdateTask(int id, int userId, [FromBody] UpdateTaskDto dto)
        {
            var task = await _taskService.UpdateAsync(id, userId, dto);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPatch("{id}/user/{userId}/status")]
        public async Task<ActionResult<TaskDto>> UpdateStatus(int id, int userId, [FromBody] TaskStatus status)
        {
            var dto = new UpdateTaskDto { Status = status };
            var task = await _taskService.UpdateAsync(id, userId, dto);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<ActionResult> DeleteTask(int id, int userId)
        {
            var deleted = await _taskService.DeleteAsync(id, userId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
