using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aa.Models;
using aa.Views;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System.Linq.Expressions;
using NuGet.Protocol.Core.Types;
using Microsoft.CodeAnalysis;


namespace aa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly SoupDbContext context;
        //private readonly IUserService userService; 

        public ProjectsController(SoupDbContext _context)
        {
            context = _context;
            //userService = _userService; 
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            return await context.Projects.Select(p => new ProjectDto(p)).ToListAsync();
        }

        // GET: api/Projects/ForDisplay/Public
        [HttpGet("ForDisplay/Public")]
        public async Task<ActionResult<IEnumerable<ProjectForDisplayDto>>> GetProjectsForDisplay()
        {
            return await (from project in context.Projects
                          where project.IsPrivate == false
                          join user in context.Users
                            on project.Creator equals user.Id
                          select (new ProjectForDisplayDto(project, user.Name)))
                          .ToListAsync();  
        }

        /* MARKED FOR DELETION, OBSOLETE
        // GET: api/Projects/Creators/5
        [HttpGet("Creators/{userId}")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetCreatorsProjects(int userId)
        {
            return await context.Projects
                .Where(p => p.Creator == userId)
                .Select(p => new ProjectDto(p))
                .ToListAsync();
        }*/

        /* MARKED FOR DELETION, OBSOLETE
        // GET: api/Projects/ForDisplay/Creators/5
        [HttpGet("ForDisplay/Creators/{userId}")]
        public async Task<ActionResult<IEnumerable<ProjectForDisplayDto>>> GetProjectsForDisplayCreators(int userId)
        {
            return await context.Projects
                .Where(p => p.Creator == userId).Join(context.Users,
                    p => p.Creator,
                    u => u.Id,
                    (p, u) => new ProjectForDisplayDto(p, u.Name)
                ).ToListAsync();
        }*/

        // GET: api/Projects/ForDisplay/Participants/5
        [HttpGet("ForDisplay/Participants/{userId}")]
        public async Task<ActionResult<IEnumerable<ProjectForDisplayDto>>> GetProjectsForDisplayParticipants(int userId)
        {
            return await (from project in context.Projects
                          join team in context.Teams
                            on project.Id equals team.ProjectId
                          where team.UserId == userId
                          join user in context.Users
                            on project.Creator equals user.Id
                          select (new ProjectForDisplayDto(project, user.Name))
                          )
                          .Distinct() 
                          .ToListAsync();  
        }

        // GET: api/Projects/Search/ForDisplay/Public
        [HttpPost("Search/ForDisplay/Public")]
        public async Task<ActionResult<IEnumerable<ProjectForDisplayDto>>> GetProjectsSearchForDisplay(ProjectForDisplayDto searchdto)
        {
            return await (from project in context.Projects
                          where project.IsPrivate == false
                          join user in context.Users on project.Creator equals user.Id
                          where project.Name.ToUpper().Contains(searchdto.Name.ToUpper())
                                && project.Description.ToUpper().Contains(searchdto.Description.ToUpper())
                                && user.Name.ToUpper().Contains(searchdto.CreatorName.ToUpper())
                          select new ProjectForDisplayDto(project, user.Name)) 
                           .ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            var project = await context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            return new ProjectDto(project);
        }

        // GET: api/Projects/ForDisplay/5
        [HttpGet("ForDisplay/{id}")]
        public async Task<ActionResult<ProjectForDisplayDto>> GetProjectForDisplay(int id)
        {
            var project = await context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            var creator = await context.Users.FindAsync(project.Creator);

            if (creator == null)
                return NotFound();

            return new ProjectForDisplayDto(project, creator.Name);
        }


        // PUT          -- changes name, description & satedeadline only  
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> PutProject(int id, ProjectDto projectdto)
        {
            if (id != projectdto.Id)
            {
                return BadRequest();
            }

            var project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Name = projectdto.Name;
            project.Description = projectdto.Description; 
            project.DateDeadline = DateTime.Parse(projectdto.DateDeadline); 
            project.IsPrivate = projectdto.IsPrivate; 

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ProjectExists(id))
            {
                return NotFound();
            }

            return Ok(); 
        }


        // PUT          -- finishes project 
        [HttpPut("Finish/{id}")]
        public async Task<ActionResult<ProjectDto>> FinishProject(int id, ProjectDto projectdto)
        {
            if (id != projectdto.Id)
            {
                return BadRequest();
            }

            var project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.IsComplete = true; 
            project.DateFinished = DateTime.Now; 
         
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ProjectExists(id))
            {
                return NotFound();
            }

            return Ok(); 
        }


        // POST
        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto projectdto)
        {
            //creating project 
            var project = new aa.Models.Project
            {
                Name = projectdto.Name,
                Description = projectdto.Description,
                Creator = projectdto.Creator, 
                DateBegan = DateTime.Now, 
                DateDeadline = (projectdto.DateDeadline == "")?null:DateTime.Parse(projectdto.DateDeadline),   
                IsPrivate = projectdto.IsPrivate 
            };

            context.Projects.Add(project);

            await context.SaveChangesAsync();

            //creating team
            project = context.Projects.FirstOrDefault(p => p.Name == projectdto.Name);

            var team = new Team
            {
                UserId = project.Creator, //implies creatorid, ofc. i'm a dummy and messed up the dtos 
                ProjectId = project.Id,
                Role = "Руководитель проекта",
                Level = 0,
            };

            context.Teams.Add(team);

            await context.SaveChangesAsync();

            return Ok(); 
        }

        // POST
        [HttpPost("AttachRepository/{projectId}")]
        public async Task<IActionResult> AttachRepository(RepositoryDto repositorydto, int projectId)
        {
            //creating repository
            var repository = new aa.Models.Repository
            {
                Id = projectId, 
                GithubName = repositorydto.GithubName, 
                GithubCreator = repositorydto.GithubCreator 
            };

            context.Repositories.Add(repository);

            await context.SaveChangesAsync();

            return NoContent();
        }


        private bool ProjectExists(int id)
        {
            return context.Projects.Any(e => e.Id == id);
        }
    }
}
