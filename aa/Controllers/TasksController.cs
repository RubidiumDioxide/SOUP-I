using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aa.Models;
using aa.Views; 

namespace aa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly SoupDbContext context;

        public TasksController(SoupDbContext _context)
        {
            context = _context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            return await context.Tasks
                .Select(t => new TaskDto(t))
                .ToListAsync();
        }

        // GET: api/Tasks/ByProject/5
        [HttpGet("ByProject/{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByProject(int projectId)
        {
            return await context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Select(t => new TaskDto(t))
                .ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var task = await context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return new TaskDto(task); 
        }

        // GET: api/Tasks/Ratio/ByProject/5
        [HttpGet("Ratio/ByProject/{projectId}")]
        public async Task<ActionResult<double>> GetTasksRatioByProject(int projectId)
        {
            var done = await (from task in context.Tasks
                              where task.ProjectId == projectId && task.IsComplete
                              select task.Id).ToListAsync();

            var all = await (from task in context.Tasks
                             where task.ProjectId == projectId 
                             select task.Id).ToListAsync();

            return done.Count / all.Count; 
        }


        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskDto taskdto)
        {
            if (id != taskdto.Id)
            {
                return BadRequest();
            }

            var task = await context.Tasks.FindAsync(id); 

            if (task == null)
            {
                return BadRequest();
            }

            task.ProjectId = taskdto.ProjectId; 
            task.CreatorId = taskdto.CreatorId; 
            task.AssigneeId = taskdto.AssigneeId; 
            task.Name = taskdto.Name; 
            task.Description = taskdto.Description; 
            task.IsComplete = taskdto.IsComplete;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TaskExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Tasks/Complete/5
        [HttpPut("Complete/{id}")]
        public async Task<IActionResult> PutTaskComplete(int id)
        {
            var task = await context.Tasks.FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            task.IsComplete = true; 

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TaskExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }


        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostTask(TaskDto taskdto)
        {
            var task = new aa.Models.Task
            {
                ProjectId = taskdto.ProjectId,
                CreatorId = taskdto.CreatorId,
                AssigneeId = taskdto.AssigneeId,
                Name = taskdto.Name,
                Description = taskdto.Description,
                IsComplete = taskdto.IsComplete
            };

            context.Tasks.Add(task);

            await context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Tasks/Lea 
        [HttpPost("{userName}")]
        public async Task<IActionResult> PostTaskByAssignee(TaskDto taskdto, string userName)
        {
            var assignee = await context.Users.FirstOrDefaultAsync(u => u.Name == userName);

            if (assignee == null)
            {
                return NotFound(); 
            }

            var task = new aa.Models.Task
            {
                ProjectId = taskdto.ProjectId,
                CreatorId = taskdto.CreatorId,
                AssigneeId = assignee.Id,
                Name = taskdto.Name,
                Description = taskdto.Description,
                IsComplete = false 
            };

            context.Tasks.Add(task);

            await context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return context.Tasks.Any(e => e.Id == id);
        }


        // FOR DISPLAY

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskForDisplayDto>>> GetTasksForDisplay()
        {
            return await (from task in context.Tasks
                          join project in context.Projects on task.ProjectId equals project.Id
                          join creator in context.Users on task.CreatorId equals creator.Id
                          join assignee in context.Users on task.AssigneeId equals assignee.Id
                          select new TaskForDisplayDto
                          {
                              Id = task.Id,
                              ProjectId = task.ProjectId, 
                              ProjectName = project.Name, 
                              CreatorId = creator.Id,
                              CreatorName = creator.Name,
                              AssigneeId = assignee.Id,
                              AssigneeName = assignee.Name,
                              Name = task.Name,
                              Description = task.Description,
                              IsComplete = task.IsComplete
                          }).ToListAsync();
        }

        // GET: api/Tasks/ByProject/ForDisplay/5
        [HttpGet("ByProject/ForDisplay/{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskForDisplayDto>>> GetTasksByProjectForDisplay(int projectId)
        {
            return await (from task in context.Tasks
                         where task.ProjectId == projectId
                         join project in context.Projects on task.ProjectId equals project.Id
                         join creator in context.Users on task.CreatorId equals creator.Id
                         join assignee in context.Users on task.AssigneeId equals assignee.Id
                         select new TaskForDisplayDto {
                             Id = task.Id,
                             ProjectId = task.ProjectId, 
                             ProjectName = project.Name, 
                             CreatorId = creator.Id,
                             CreatorName = creator.Name,
                             AssigneeId = assignee.Id,
                             AssigneeName = assignee.Name,
                             Name = task.Name,
                             Description = task.Description,
                             IsComplete = task.IsComplete
                         }).ToListAsync(); 
        }

        // GET: api/Tasks/ByAssignee/ForDisplay/5
        [HttpGet("ByAssignee/ForDisplay/{assigneeId}")]
        public async Task<ActionResult<IEnumerable<TaskForDisplayDto>>> GetTasksByUserForDisplay(int assigneeId)
        {
            return await (from task in context.Tasks
                          where task.AssigneeId == assigneeId && task.IsComplete == false
                          join project in context.Projects on task.ProjectId equals project.Id
                          join creator in context.Users on task.CreatorId equals creator.Id
                          join assignee in context.Users on task.AssigneeId equals assignee.Id
                          select new TaskForDisplayDto
                          {
                              Id = task.Id,
                              ProjectId = task.ProjectId, 
                              ProjectName = project.Name, 
                              CreatorId = creator.Id,
                              CreatorName = creator.Name,
                              AssigneeId = assignee.Id,
                              AssigneeName = assignee.Name,
                              Name = task.Name,
                              Description = task.Description,
                              IsComplete = task.IsComplete
                          }).ToListAsync();
        }


        // GET: api/Tasks/ForDisplay/5
        [HttpGet("ForDisplay/{id}")]
        public async Task<ActionResult<TaskForDisplayDto>> GetTaskForDisplay(int id)
        {
            var task = await context.Tasks.FindAsync(id); 

            if (task == null)
            {
                return NotFound();
            }

            var project = await context.Projects.FindAsync(task.ProjectId); 
            var creator = await context.Users.FindAsync(task.CreatorId);
            var assignee = await context.Users.FindAsync(task.AssigneeId);

            if (creator == null || assignee == null || project == null)
            {
                return NotFound();
            }

            return new TaskForDisplayDto
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                ProjectName = project.Name, 
                CreatorId = creator.Id,
                CreatorName = creator.Name,
                AssigneeId = assignee.Id,
                AssigneeName = assignee.Name,
                Name = task.Name,
                Description = task.Description,
                IsComplete = task.IsComplete
            }; 
        }
    }
}
