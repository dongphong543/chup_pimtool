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
        private readonly EmployeeRepository _employeeRepository;
        public ProjectRepository(PIMContext context) : base(context)
        {
            _employeeRepository = new EmployeeRepository(context);

        }

        public bool ProjectNumberExists(decimal pjNum)
        {
            return Set.Any<Project>(p => p.ProjectNumber == pjNum);
        }

        public void AddMemberChange(string memString, Project project)
        {
            if (string.IsNullOrEmpty(memString) == false)
            {
                string[] notFoundVisa = _employeeRepository.GetExistOrNotExistEmployeeArray(memString, false);
                if (notFoundVisa.Length == 0)
                {
                    string[] foundVisa = _employeeRepository.GetExistOrNotExistEmployeeArray(memString, true);
                    for (int i = 0; i < foundVisa.Length; ++i)
                    {
                        project.Employees.Add(_employeeRepository.GetByVisa(foundVisa[i]));
                    }
                }

                else
                {
                    throw new VisaInvalidException("Visa invalid.");
                }
            }

            Set.Add(project);
        }

        public void UpdateMemberChange(string memString, Project projectDb)
        {
            if (string.IsNullOrEmpty(memString) == false)
            {
                string[] notFoundVisa = _employeeRepository.GetExistOrNotExistEmployeeArray(memString, false);

                if (notFoundVisa.Length == 0)
                {
                    projectDb.Employees.Clear();
                    string[] foundVisa = _employeeRepository.GetExistOrNotExistEmployeeArray(memString, true);
                    for (int i = 0; i < foundVisa.Length; ++i)
                    {
                        projectDb.Employees.Add(_employeeRepository.GetByVisa(foundVisa[i]));
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