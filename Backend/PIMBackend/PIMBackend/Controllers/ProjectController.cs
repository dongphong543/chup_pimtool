using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMBackend.DTOs;
using PIMBackend.Errors;

namespace PIMBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly PIMContext _context;

        public ProjectController(PIMContext context)
        {
            _context = context;
        }

        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(decimal id)
        {
            var Project = await _context.Projects.FindAsync(id);

            if (Project == null)
            {
                throw new IdNotExistException();
            }

            return Project;
        }

        [HttpGet("pjNum/{pjNum}")]
        public async Task<ActionResult<Project>> GetProjectByPjNum(decimal pjNum)
        {
            var Project = await _context.Projects.Where(x => x.ProjectNumber == pjNum).ToListAsync();

            if (Project == null || Project.Count == 0)
            {
                throw new ProjectNumberNotExistException();
            }

            return Project[0];
        }

        [HttpGet("exist/{pjNum}")]
        public async Task<ActionResult<bool>> CheckProjectByPjNum(decimal pjNum)
        {
            var Project = await _context.Projects.Where(x => x.ProjectNumber == pjNum).ToListAsync();

            if (Project == null || Project.Count == 0)
            {
                return false;
            }

            return true;
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(decimal id, Project Project)
        {
            if (id != Project.Id)
            {
                throw new IdNotEqualsProjcetIdException();
            }

            var pj = await _context.Projects.FindAsync(id);

            if (pj == null)
            {
                throw new IdNotExistException();
            }

            if (Project.StartDate > Project.EndDate)
            {
                throw new DateInvalidException();
            }

            if (Project.ProjectNumber < 0 || Project.ProjectNumber > 9999 ||
                Project.Name.Length > 50 ||
                Project.Customer.Length > 50)
            {
                throw new FormInvalidException();
            }

            if (Project.Version == pj.Version)
            {
                //Console.WriteLine(Project.Version.ToString(), pj.Version.ToString());
                Project.Version = pj.Version + 1;
            }
            else
            {
                throw new DbUpdateConcurrencyException();
            }

            _context.Entry(pj).State = EntityState.Detached;
            _context.Entry(Project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        
            return NoContent();
        }

        // POST: api/Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project Project)
        {
            var pj = this.ProjectExistsByPjNum(Project.ProjectNumber);

            if (pj == true)
            {
                throw new ProjectNumberAlreadyExistsException();
            }

            if (Project.StartDate > Project.EndDate)
            {
                throw new DateInvalidException();
            }

            if (Project.ProjectNumber < 0 || Project.ProjectNumber > 9999 ||
                Project.Name.Length > 50 ||
                Project.Customer.Length > 50)
            {
                throw new FormInvalidException();
            }

            _context.Projects.Add(Project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = Project.Id }, Project);
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(decimal id)
        {
            var Project = await _context.Projects.FindAsync(id);
            if (Project == null)
            {
                throw new IdNotExistException();
            }

            _context.Projects.Remove(Project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(decimal id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        private bool ProjectExistsByPjNum(decimal pjNum)
        {
            return _context.Projects.Any(e => e.ProjectNumber == pjNum);
        }
    }
}