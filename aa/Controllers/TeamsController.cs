using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aa.Models;
using aa.Views;
using Microsoft.AspNetCore.Http.HttpResults;

namespace aa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly SoupDbContext context;

        Dictionary<string, string[]> Roles = new Dictionary<string, string[]>(){
            { "Руководитель проекта",
                [ "Руководитель отдела дизайна",
                "Руководитель отдела разработки",
                "Руководитель отдела внедрения и тестирования",
                "Руководитель отдела информационной безопасности",
                "Руководитель отдела аналитики" ]
            },
            {
                "Руководитель отдела дизайна",
                [ "Веб-дизайнер", 
                "Графический дизайнер", 
                "3D-дизайнер", 
                "UI/UX дизайнер" ]
            }, 
            {
                "Руководитель отдела разработки",
                [ "Архитектор",
                "Fullstack-разработчик",
                "Front end-разработчик",
                "Back end-разработчик",
                "Разработчик баз данных" ] 
            },
            {
                "Руководитель отдела внедрения и тестирования", 
                [ "DevOps-инженер",
                "Тестировщик" ]
            },
            {
                "Руководитель отдела информационной безопасности",
                [ "Специалист по безопасности", 
                "Системный администратор" ]
            },
            {
                "Руководитель отдела аналитики",
                [ "Бизнес-аналитик", 
                "Системный аналитик" ]
            },
        }; 

        private string getUpper(string role)
        {
            return Roles.FirstOrDefault(x => x.Value.Contains(role)).Key;
        }

        private readonly List<string>[] role_levels = new List<string>[]
        {
            new List<string> { "Руководитель проекта" },
            new List<string>
            {
                "Руководитель отдела дизайна",
                "Руководитель отдела разработки",
                "Руководитель отдела внедрения и тестирования",
                "Руководитель отдела информационной безопасности",
                "Руководитель отдела аналитики"
            },
            new List<string> 
            { 
                "Веб-дизайнер", 
                "Графический дизайнер", 
                "3D-дизайнер", 
                "UI/UX дизайнер", 
                "Архитектор", 
                "Fullstack-разработчик", 
                "Front end-разработчик", 
                "Back end-разработчик", 
                "Разработчик баз данных", 
                "DevOps-инженер", 
                "Тестировщик", 
                "Специалист по безопасности", 
                "Системный администратор", 
                "Бизнес-аналитик", 
                "Системный аналитик" 
            }
        };

        public TeamsController(SoupDbContext _context)
        {
            context = _context;
        }

        // GET: api/Teams/ForDisplay
        [HttpGet("ForDisplay")]
        public async Task<ActionResult<IEnumerable<TeamForDisplayDto>>> GetTeams()
        {
            return await (from team in context.Teams
                          join user in context.Users on team.UserId equals user.Id
                          join project in context.Projects on team.ProjectId equals project.Id
                          select new TeamForDisplayDto(team, user.Name, project.Name))
                          .ToListAsync();
        }

        // GET: api/Teams/ForDisplay/Project/5
        [HttpGet("ForDisplay/Project/{projectId}")]
        public async Task<ActionResult<IEnumerable<TeamForDisplayDto>>> GetTeamsProjects(int projectId)
        {
            return await (from team in context.Teams
                          where team.ProjectId == projectId 
                          join user in context.Users on team.UserId equals user.Id
                          join project in context.Projects on team.ProjectId equals project.Id
                          select new TeamForDisplayDto(team, user.Name, project.Name))
                          .ToListAsync();
        }

        // GET: api/Teams/ForDisplay/5
        [HttpGet("ForDisplay/{id}")]
        public async Task<ActionResult<TeamForDisplayDto>> GetTeam(int id)
        {
            var team = await context.Teams.FindAsync(id);

            if(team == null)
            {
                return NotFound(); 
            }

            var project = await context.Projects.FindAsync(team.ProjectId);
            var user = await context.Users.FindAsync(team.UserId);

            if (project == null || user == null)
            {
                return NotFound();
            }

            return new TeamForDisplayDto(team, user.Name, project.Name);
        }

        // GET: api/Teams/AssignableTeammates/ByUserProject/ForDisplay/5/3 
        [HttpGet("AssignableTeammates/ByUserProject/ForDisplay/{userId}/{projectId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetAssignableTeammatesForDisplay(int userId, int projectId)
        {
            var user_as_teammate = await (from team in context.Teams
                                          where (team.UserId == userId && team.ProjectId == projectId)
                                          orderby team.Level ascending
                                          select new TeamDto(team))
                                          .FirstOrDefaultAsync();

            if (user_as_teammate == null)
                return BadRequest();

         
            return await (from team in context.Teams
                        where team.ProjectId == projectId && team.Level >= user_as_teammate.Level
                        join teammate in context.Users on team.UserId equals teammate.Id
                        select teammate.Name)
                        .Distinct()
                        .ToListAsync();

            return NotFound();
        }


        // PUT: api/Teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDto teamdto)
        {
            if (id != teamdto.Id)
            {
                return BadRequest();
            }

            var team = await context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound(); 
            }

            if(team.Level == 0)
            {
                return BadRequest(); 
            }

            team.Role = teamdto.Role;  //in case of changing role specifically; if there's any need to change all, rewrite method 
            team.Level = Array.FindIndex(role_levels, level => level.Contains(teamdto.Role));

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (! TeamExists(id))
            {
              return NotFound();
            }

            return NoContent(); 
        }

 
        // POST: api/Teams
        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamForDisplayDto teamdto)
        {
            var team = new Team
            {
                UserId = teamdto.UserId,
                ProjectId = teamdto.ProjectId,
                Role = teamdto.Role, 
                Level = Array.FindIndex(role_levels, level => level.Contains(teamdto.Role)) 
            }; 

            context.Teams.Add(team);

            await context.SaveChangesAsync();

            return NoContent(); 
        }

        // POST: api/Teams/PostByName
        [HttpPost("PostByName")]
        public async Task<IActionResult> CreateTeamByName(TeamForDisplayDto teamdto)
        {
            var user = context.Users.FirstOrDefault(u => u.Name == teamdto.UserName);

            if (user == null)
            {
                return NotFound(); 
            }

            var team = new Team
            {
                UserId = user.Id,
                ProjectId = teamdto.ProjectId,
                Role = teamdto.Role, 
                Level = Array.FindIndex(role_levels, level => level.Contains(teamdto.Role))
            };

            context.Teams.Add(team);

            await context.SaveChangesAsync();

            return NoContent();
        }

        
        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await context.Teams.FindAsync(id);
            
            if (team == null)
            {
                return NotFound();
            }

            if(team.Level == 0)
            {
                return BadRequest("Нельзя выгнать из клманды руководителя проекта"); 
            }

            context.Teams.Remove(team);
            await context.SaveChangesAsync();

            return NoContent();
        }


        private bool TeamExists(int id)
        {
            return context.Teams.Any(t => t.Id == id);
        }
    }
}
