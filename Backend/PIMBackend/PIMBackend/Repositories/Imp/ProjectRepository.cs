using Microsoft.EntityFrameworkCore;
using PIMBackend.Database;
using PIMBackend.Domain.Entities;
using PIMBackend.Errors;
using System.Collections.Generic;
using System.Linq;

namespace PIMBackend.Repositories.Imp
{
    /// <summary>
    ///     The implementation of sample repository
    /// </summary>
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly EmployeeRepository _emRepo;
        public ProjectRepository(PIMContext context) : base(context)
        {
            _emRepo = new EmployeeRepository(context);

        }

        //public Employee GetByVisa(string visa)
        //{
        //    return _context.Set<Employee>().SingleOrDefault(x => x.Visa == visa);
        //}

        public void AddWithMem(string memString, Project project)
        {
            string[] notFoundVisa = _emRepo.GetExistEm(memString, false);
            if (notFoundVisa.Length == 0)
            {
                string[] foundVisa = _emRepo.GetExistEm(memString, true);
                for (int i = 0; i < foundVisa.Length; ++i)
                {
                    project.Employees.Add(_emRepo.GetByVisa(foundVisa[i]));
                }
            }

            else
            {
                throw new VisaInvalidException();
            }

            Set.Add(project);
        }

        public void UpdateWithMem(string memString, Project projectDb)
        {
            string[] notFoundVisa = _emRepo.GetExistEm(memString, false);

            if (notFoundVisa.Length == 0)
            {   
                projectDb.Employees.Clear();
                string[] foundVisa = _emRepo.GetExistEm(memString, true);
                for (int i = 0; i < foundVisa.Length; ++i)
                {
                    projectDb.Employees.Add(_emRepo.GetByVisa(foundVisa[i]));
                }
            }

            else
            {
                throw new VisaInvalidException();
            }

            Set.Update(projectDb);

        }

        //public void Delete(decimal[] ids)
        //{
        //    Delete(ids);
        //}

    }
}