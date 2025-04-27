//rework!!! 

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
    public class RepositoriesController : ControllerBase
    {
        private readonly SoupDbContext context;

        public RepositoriesController(SoupDbContext _context)
        {
            context = _context;
        }

        // GET: api/Repositories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepositoryDto>>> GetRepositories()
        {
            return await context.Repositories
                .Select(r => new RepositoryDto(r))
                .ToListAsync();
        }

        // GET: api/Repositories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RepositoryDto>> GetRepository(int id)
        {
            var repository = await context.Repositories.FindAsync(id);

            if (repository == null)
                return NotFound();

            return new RepositoryDto(repository); 
        }

        // will very likely go unused 
        // PUT: api/Repositories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepository(int id, RepositoryDto repositorydto)
        {
            if (id != repositorydto.Id)
                return BadRequest(); 

            var repository = await context.Repositories.FindAsync(id); 

            if (repository == null)
                return BadRequest();

            repository.GithubName = repositorydto.GithubName; 
            repository.GithubCreator = repositorydto.GithubCreator; 

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (! RepositoryExists(id))
            {
                return NotFound(); 
            }

            return NoContent();
        }

        // POST: api/Repositories
        [HttpPost]
        public async Task<IActionResult> PostRepository(RepositoryDto repositorydto)
        {
            var repository = new Repository {
                GithubName = repositorydto.GithubName,
                GithubCreator = repositorydto.GithubCreator, 
            };

            context.Repositories.Add(repository); 

            await context.SaveChangesAsync();

            return Ok(); 
        }

        // DELETE: api/Repositories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepository(int id)
        {
            var repository = await context.Repositories.FindAsync(id);

            if (repository == null)
                return NotFound();

            context.Repositories.Remove(repository);
            await context.SaveChangesAsync();

            return Ok(); 
        }

        private bool RepositoryExists(int id)
        {
            return context.Repositories.Any(e => e.Id == id);
        }
    }
}
