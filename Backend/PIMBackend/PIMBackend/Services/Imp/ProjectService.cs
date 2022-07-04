using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PIMBackend.Domain.Entities;
using PIMBackend.DTOs;
using PIMBackend.Errors;
using PIMBackend.Repositories;

namespace PIMBackend.Services.Imp
{
    /// <summary>
    ///     Implementation of project service
    /// </summary>
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        protected readonly IEmployeeRepository _employeeRepository;

        public ProjectService(IProjectRepository projectRepository, IEmployeeRepository employeeRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<Project> Get(string searchText, string searchStatus)
        {

            // Get the query
            IQueryable<Project> query = _projectRepository.Get().Include(p => p.Employees);
            

            // Search by searching criteria
            if (string.IsNullOrEmpty(searchText) == false)
            {
                query = query.Where(p =>
                                        p.ProjectNumber.ToString().Contains(searchText) ||
                                        p.Name.Contains(searchText) ||
                                        p.Customer.Contains(searchText)
                                    );
            }

            if (string.IsNullOrEmpty(searchStatus) == false)
            {
                query = query.Where(p =>
                                        (p.Status.Contains(searchStatus))
                                    );
            }

            return query;
        }


        public Project Get(decimal id)
        {
            var ret = _projectRepository.Get().Include(p => p.Employees).SingleOrDefault(x => x.Id == id);
            if (ret == null)
            {
                throw new IdNotExistException("Id not exist.", id);
            }
            return ret;
        }

        public Project GetByPjNum(decimal pjNum)
        {
            var ret = _projectRepository.Get().Include(p => p.Employees).SingleOrDefault(x => x.ProjectNumber == pjNum);
            if (ret == null)
            {
                throw new ProjectNumberNotExistsException("Project number not exist.", pjNum);
            }
            return ret;
        }
        
        private void ValidateForm(Project project)
        {
            if (project.EndDate != null && project.StartDate > project.EndDate)
            {
                throw new DateInvalidException("Invalid date.", project.StartDate, (DateTime)project.EndDate);
            }

            if (project.ProjectNumber < 0 || project.ProjectNumber > 9999 ||
                string.IsNullOrWhiteSpace(project.Name) || project.Name.Length > 50 ||
                string.IsNullOrWhiteSpace(project.Customer) || project.Customer.Length > 50 ||
                string.IsNullOrWhiteSpace(project.Status))
            {
                throw new FormInvalidException();
            }
        }

        public void Create(Project project, string memString)
        {
            if (ProjectNumberExists(project.ProjectNumber) == true)
            {
                throw new ProjectNumberAlreadyExistsException("Project number(s) already exist(s).", project.ProjectNumber);
            }

            ValidateForm(project);

            _projectRepository.AddMemberChange(memString, project);

            try
            {
                _projectRepository.SaveChange();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new UpdateConflictException("Conflict in create.", e);
            }
            
        }

        public Project Update(Project project, string memString)
        {
            var projectDb = _projectRepository.Get().Include(projects => projects.Employees)
                .SingleOrDefault(p => p.Id == project.Id);

            if (projectDb == null)
            {
                throw new UpdateConflictException("Conflict", null);
            }

            if (project.Version.SequenceEqual(projectDb.Version) == false)
            {
                throw new UpdateConflictException("Conflict", null);
            }

            ValidateForm(project);

            if (project.ProjectNumber != projectDb.ProjectNumber)
            {
                throw new FormInvalidException();
            }

            else
            {
                projectDb.GroupId = project.GroupId;
                projectDb.Name = project.Name;
                projectDb.Customer = project.Customer;
                projectDb.Status = project.Status;
                projectDb.StartDate = project.StartDate;
                projectDb.EndDate = project.EndDate;

                _projectRepository.UpdateMemberChange(memString, projectDb);
            }

            try
            {
                _projectRepository.SaveChange();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new UpdateConflictException("Conflict in update.", e);
            }

            return projectDb;
        }

        public void Delete(decimal[] pjNums)
        {
            
            List<decimal> ids = new List<decimal>();

            for (int i = 0; i < pjNums.Length; ++i)
            {
                var project = GetByPjNum(pjNums[i]);
                if (project == null)
                {
                    throw new UpdateConflictException("Conflict  in delete.", null);
                }
                if (project.Status != "NEW")
                {
                    throw new StatusInvalidException("Status invalid", project.Status);
                }
                ids.Add(project.Id);
            }

            _projectRepository.Delete(ids.ToArray());
            _projectRepository.SaveChange();
        }

        public bool ProjectNumberExists(decimal pjNum)
        {
            return _projectRepository.ProjectNumberExists(pjNum);
        }
    }
}