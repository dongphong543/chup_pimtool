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

        public bool ProjectNumberExists(decimal pjNum)
        {
            return Set.Any<Project>(p => p.ProjectNumber == pjNum);
        }

        public void Add(string memString, Project project)
        {
            if (string.IsNullOrEmpty(memString) == false)
            {
                string[] notFoundVisa = _emRepo.GetExistOrNotExistEmployeeArray(memString, false);
                if (notFoundVisa.Length == 0)
                {
                    string[] foundVisa = _emRepo.GetExistOrNotExistEmployeeArray(memString, true);
                    for (int i = 0; i < foundVisa.Length; ++i)
                    {
                        project.Employees.Add(_emRepo.GetByVisa(foundVisa[i]));
                    }
                }

                else
                {
                    throw new VisaInvalidException("Visa invalid.");
                }
            }

            Set.Add(project);
        }

        public void Update(string memString, Project projectDb)
        {
            if (string.IsNullOrEmpty(memString) == false)
            {
                string[] notFoundVisa = _emRepo.GetExistOrNotExistEmployeeArray(memString, false);

                if (notFoundVisa.Length == 0)
                {
                    projectDb.Employees.Clear();
                    string[] foundVisa = _emRepo.GetExistOrNotExistEmployeeArray(memString, true);
                    for (int i = 0; i < foundVisa.Length; ++i)
                    {
                        projectDb.Employees.Add(_emRepo.GetByVisa(foundVisa[i]));
                    }
                }

                else
                {
                    throw new VisaInvalidException("Visa invalid.");
                }
            }
            

            Set.Update(projectDb);

        }


    }
}