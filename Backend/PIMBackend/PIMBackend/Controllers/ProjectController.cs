using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMBackend.DTOs;

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
                return NotFound();
            }

            return Project;
        }

        [HttpGet("pjNum/{pjNum}")]
        public async Task<ActionResult<Project>> GetProjectByPjNum(decimal pjNum)
        {
            //var Project = await _context.Projects.FindAsync(id);
            var Project = await _context.Projects.Where(x => x.ProjectNumber == pjNum).ToListAsync();

            if (Project == null || Project.Count == 0)
            {
                return NotFound();
            }

            return Project[0];
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(decimal id, Project Project)
        {
            if (id != Project.Id)
            {
                return BadRequest();
            }


            try
            {
                //var pj = await GetProjectByPjNum(Project.ProjectNumber);
                //if (Project.Version != pj.R)
                //{
                //    Project.Version += 1;
                //}

                var pj = await _context.Projects.FindAsync(id);

                if (pj == null)
                {
                    return NotFound();
                }

                if (Project.Version == pj.Version)
                {
                    Console.WriteLine(Project.Version.ToString(), pj.Version.ToString());
                    Project.Version = pj.Version + 1;
                }
                else
                {
                    throw new DbUpdateConcurrencyException();
                }

                _context.Entry(pj).State = EntityState.Detached;
                _context.Entry(Project).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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
                return BadRequest();
            }

            try
            {
                _context.Projects.Add(Project);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
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
                return NotFound();
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