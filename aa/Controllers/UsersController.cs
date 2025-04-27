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
    public class UsersController : ControllerBase
    {
        private readonly SoupDbContext context;
        //private readonly IUserService userService; 

        public UsersController(SoupDbContext _context)
        {
            context = _context;
            //userService = _userService; 
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await context.Users.Select(u => new UserDto(u)).ToListAsync();
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDto(user);
        }

        // GET: api/Users/Find/5
        [HttpGet("/api/Users/Find/{name}")]
        public async Task<ActionResult<UserDto>> GetUser(string name)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == name); 

            if (user == null)
            {
                return NotFound();
            }

            return new UserDto(user);
        }

        // GET: api/Users/IsInTeam/3/5
        [HttpGet("IsInTeam/{userId}/{projectId}")]
        public async Task<IActionResult> IsUserInTeam(int userId, int projectId)
        {
            var user = await context.Teams
                .Where(t => ((t.UserId == userId) && (t.ProjectId == projectId)))
                .ToListAsync(); 

            if (user.Count() == 0)
            {
                return NotFound();
            }

            return Ok(); 
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> PutUser(int id, UserDto userdto)
        {
            if (id != userdto.Id)
            {
                return BadRequest();
            }

            var user = await context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound(); 
            }

            user.Name = userdto.Name; 
            user.Password = userdto.Password; 

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
            {
                return NotFound();
            }

            return new UserDto(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userdto)
        {
            var user = new User 
            {
                Name = userdto.Name, 
                Password = userdto.Password 
            };

            context.Users.Add(user); 
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser), new { id = user.Id}, new UserDto(user));
        } 

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id); 
            if(user == null)
            {
                return NotFound(); 
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return new UserDto(user); 
        }

        private bool UserExists(int id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
