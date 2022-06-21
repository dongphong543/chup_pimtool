using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PIMBackend.DTOs;
using PIMBackend.Errors;

namespace PIMBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectEmployeeController : ControllerBase
    {
        private readonly PIMContext _context;

        public ProjectEmployeeController(PIMContext context)
        {
            _context = context;
        }

        // GET: api/ProjectEmployee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectEmployee>>> GetProjectEmployees()
        {
            return await _context.ProjectEmployees.ToListAsync();
        }

        // GET: api/ProjectEmployee/5
        [HttpGet("{pjId}_{emId}")]
        public async Task<ActionResult<ProjectEmployee>> GetProjectEmployee(decimal pjId, decimal emId)
        {
            var ProjectEmployee = await _context.ProjectEmployees.FindAsync(pjId, emId);

            if (ProjectEmployee == null)
            {
                return NotFound();
            }

            return ProjectEmployee;
        }

        // PUT: api/ProjectEmployee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{pjId}_{emId}")] // [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectEmployee(decimal pjId, decimal emId, ProjectEmployee ProjectEmployee)
        {
            if (pjId != ProjectEmployee.ProjectId || emId != ProjectEmployee.EmployeeId)
            {
                throw new IdNotExistException();
            }

            _context.Entry(ProjectEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectEmployeeExists(pjId, emId))
                {
                    throw new IdNotExistException();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProjectEmployee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectEmployee>> PostProjectEmployee(PjNumVisa PjNumVisa)
        {
            decimal PjId = -1, EmId = -1;

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    PjId = GetIdFromProjectNumber(PjNumVisa.ProjectPjNum);
                    EmId = GetIdFromVisa(PjNumVisa.EmployeeVisa);

                    _context.ProjectEmployees.Add(new ProjectEmployee(PjId, EmId));
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return CreatedAtAction("GetProjectEmployee", new { 
                pjId = PjId, emId = EmId
            }, PjNumVisa);
        }

        // DELETE: api/ProjectEmployee/5
        [HttpDelete("{pjId}_{emId}")]
        public async Task<IActionResult> DeleteProjectEmployee(decimal pjId, decimal emId)
        {
            var ProjectEmployee = await _context.ProjectEmployees.FindAsync(pjId, emId);
            if (ProjectEmployee == null)
            {
                return NotFound();
            }

            _context.ProjectEmployees.Remove(ProjectEmployee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectEmployeeExists(decimal pjId, decimal emId)
        {
            return _context.ProjectEmployees.Any(e => e.ProjectId == pjId && e.EmployeeId == emId);
        }

        private decimal GetIdFromProjectNumber(decimal pjNum) 
        {
            var Project = _context.Projects.Where(x => x.ProjectNumber == pjNum).ToList();

            if (Project == null || Project.Count == 0)
            {
                throw new ProjectNumberNotExistsException();
            }

            return Project[0].Id;
        }

        private decimal GetIdFromVisa(string visa)
        {
            var Employee = _context.Employees.Where(x => x.Visa == visa).ToList();

            if (Employee == null || Employee.Count == 0)
            {
                throw new ProjectNumberNotExistsException();
            }

            return Employee[0].Id;
        }
    }
}