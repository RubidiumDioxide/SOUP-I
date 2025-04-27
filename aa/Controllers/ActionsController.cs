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
    public class ActionsController : ControllerBase
    {
        private readonly SoupDbContext context;

        public ActionsController(SoupDbContext _context)
        {
            context = _context;
        }

        // GET: api/Actions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActionDto>>> GetActions()
        {
            return await context.Actions
                .Select(a => new ActionDto(a))
                .ToListAsync();
        }

        // GET: api/Actions/ByProject/5
        [HttpGet("ByProject/{projectId}")]
        public async Task<ActionResult<IEnumerable<ActionDto>>> GetActionsByProject(int projectId)
        {
            return await context.Actions
                .Where(a => a.ProjectId == projectId)
                .Select(a => new ActionDto(a))
                .ToListAsync();
        }

        // GET: api/Actions/ByActor/5
        [HttpGet("ByActor/{actorId}")]
        public async Task<ActionResult<IEnumerable<ActionDto>>> GetActionsByAction(int actorId)
        {
            return await context.Actions
                .Where(a => a.ActorId == actorId)
                .Select(a => new ActionDto(a))
                .ToListAsync();
        }

        // GET: api/Actions/ByTask/5
        [HttpGet("ByTask/{taskId}")]
        public async Task<ActionResult<IEnumerable<ActionDto>>> GetActionsByTask(int taskId)
        {
            return await context.Actions
                .Where(a => a.TaskId == taskId)
                .Select(a => new ActionDto(a))
                .ToListAsync();
        }

        // GET: api/Actions/ByActor/ByTask/5/3
        [HttpGet("ByActor/ByTask/{actorId}/{taskId}")]
        public async Task<ActionResult<IEnumerable<ActionDto>>> GetActionsByActorByTask(int actorId, int taskId)
        {
            return await context.Actions
                .Where(a => a.ActorId == actorId)
                .Where(a => a.TaskId == taskId)
                .Select(a => new ActionDto(a))
                .ToListAsync();
        }


        // GET: api/Actions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActionDto>> GetAction(int id)
        {
            var action = await context.Actions.FindAsync(id);

            if (action == null)
                return NotFound();

            return new ActionDto(action);
        }

        // PUT: api/Actions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAction(int id, ActionDto actiondto)
        {
            if (id != actiondto.Id)
                return BadRequest(); 

            var action = await context.Actions.FindAsync(id);

            if (action == null)
                return BadRequest();

            action.ProjectId = actiondto.ProjectId; 
            action.ActorId = actiondto.ActorId; 
            action.TaskId = actiondto.TaskId; 
            action.Description = actiondto.Description;
            action.Commit = (actiondto.Commit == "") ? null : actiondto.Commit; 
            action.Date = actiondto.Date;

            try
            {
                await context.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException) when (!ActionExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Actions
        [HttpPost]
        public async Task<IActionResult> PostAction(ActionDto actiondto)
        {
            var task = context.Tasks.Find(actiondto.TaskId);

            if (task == null)
                return BadRequest();
            if (task.IsComplete == true)
                return BadRequest(); 

            var action = new aa.Models.Action{ 
              ProjectId = actiondto.ProjectId, 
              ActorId = actiondto.ActorId, 
              TaskId = actiondto.TaskId, 
              Description = actiondto.Description,
              Commit = (actiondto.Commit == "")?null:actiondto.Commit, 
              Date = DateTime.Now 
            };

            context.Actions.Add(action); 

            await context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Actions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAction(int id)
        {
            var action = await context.Actions.FindAsync(id);
            
            if (action == null)
                return NotFound();

            context.Actions.Remove(action);
            await context.SaveChangesAsync();

            return Ok(); 
        }

        private bool ActionExists(int id)
        {
            return context.Actions.Any(e => e.Id == id);
        }


        // FOR DISPLAY
        // GET: api/Actions/ForDisplay
        [HttpGet("ForDisplay")]
        public async Task<ActionResult<IEnumerable<ActionForDisplayDto>>> GetActionsForDisplay()
        {
            return await (from action in context.Actions
                          join project in context.Projects on action.ProjectId equals project.Id
                          join actor in context.Users on action.ActorId equals actor.Id
                          join task in context.Tasks on action.TaskId equals task.Id
                          select new ActionForDisplayDto(action, project.Name, actor.Name, task.Name))
                          .ToListAsync();
        }

        // GET: api/Actions/ByProject/ForDisplay/5
        [HttpGet("ByProject/ForDisplay/{projectId}")]
        public async Task<ActionResult<IEnumerable<ActionForDisplayDto>>> GetActionsByProjectForDisplay(int projectId)
        {
            return await (from action in context.Actions
                          where action.ProjectId == projectId
                          join project in context.Projects on action.ProjectId equals project.Id
                          join actor in context.Users on action.ActorId equals actor.Id
                          join task in context.Tasks on action.TaskId equals task.Id
                          select new ActionForDisplayDto(action, project.Name, actor.Name, task.Name))
              .ToListAsync();
        }

        // GET: api/Actions/ByActor/ForDisplay/5
        [HttpGet("ByActor/ForDisplay/{actorId}")]
        public async Task<ActionResult<IEnumerable<ActionForDisplayDto>>> GetActionsByActionForDisplay(int actorId)
        {
            return await (from action in context.Actions
                          where action.ActorId == actorId 
                          join project in context.Projects on action.ProjectId equals project.Id
                          join actor in context.Users on action.ActorId equals actor.Id
                          join task in context.Tasks on action.TaskId equals task.Id
                          select new ActionForDisplayDto(action, project.Name, actor.Name, task.Name))
              .ToListAsync();
        }

        // GET: api/Actions/ByTask/ForDisplay/5
        [HttpGet("ByTask/ForDisplay/{taskId}")]
        public async Task<ActionResult<IEnumerable<ActionForDisplayDto>>> GetActionsByTaskForDisplay(int taskId)
        {
            return await (from action in context.Actions
                          where action.TaskId == taskId 
                          join project in context.Projects on action.ProjectId equals project.Id
                          join actor in context.Users on action.ActorId equals actor.Id
                          join task in context.Tasks on action.TaskId equals task.Id
                          select new ActionForDisplayDto(action, project.Name, actor.Name, task.Name))
              .ToListAsync();
        }

        // GET: api/Actions/ForDisplay/5
        [HttpGet("ForDisplay/{id}")]
        public async Task<ActionResult<ActionForDisplayDto>> GetActionForDisplay(int id)
        {
            var action = await context.Actions.FindAsync(id);

            if (action == null)
                return NotFound();

            var project = await context.Projects.FindAsync(action.ProjectId);
            var actor = await context.Users.FindAsync(action.ActorId);
            var task = await context.Tasks.FindAsync(action.TaskId);

            if (project == null || actor == null || task == null)
                return NotFound();

            return new ActionForDisplayDto(action, project.Name, actor.Name, task.Name);
        }
    }
}
