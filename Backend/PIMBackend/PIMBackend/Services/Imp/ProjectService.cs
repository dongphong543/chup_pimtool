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

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IEnumerable<Project> Get(string searchText, string searchStatus)
        {
            IQueryable<Project> query = _projectRepository.Get().Include(p => p.Employees);
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
                throw new IdNotExistException();
            }
            return ret;
        }

        public Project GetByPjNum(decimal pjNum)
        {
            var ret = _projectRepository.Get().Include(p => p.Employees).SingleOrDefault(x => x.ProjectNumber == pjNum);
            if (ret == null)
            {
                throw new ProjectNumberNotExistsException();
            }
            return ret;
        }
        
        private void ValidateForm(Project project)
        {
            if (project.StartDate > project.EndDate)
            {
                throw new DateInvalidException();
            }

            if (project.ProjectNumber < 0 || project.ProjectNumber > 9999 ||
                project.Name == null || project.Name.Length > 50 ||
                project.Customer == null || project.Customer.Length > 50 ||
                project.Status == null)
            {
                throw new FormInvalidException();
            }
        }


        public void Create(Project project)
        {
            CreateWithMem(project, "");
        }

        
        public void CreateWithMem(Project project, string memString)
        {
            if (ProjectNumberExists(project.ProjectNumber) == true)
            {
                throw new ProjectNumberAlreadyExistsException();
            }

            
            project.Version = 0;
            ValidateForm(project);

            _projectRepository.AddWithMem(memString, project);

            try
            {
                _projectRepository.SaveChange();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new UpdateConflictException();
            }
            
        }

        public Project Update(Project project, string memString)
        {
            var projectDb = _projectRepository.Get().Include(projects => projects.Employees)
                .SingleOrDefault(p => p.Id == project.Id);

            if (projectDb == null)
            {
                throw new IdNotExistException();
            }

            if (project.Version != projectDb.Version)
            {
                throw new UpdateConflictException();
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

                projectDb.Version += 1;

                if (string.IsNullOrEmpty(memString) == false)
                {
                    _projectRepository.UpdateWithMem(memString, projectDb);
                }

            }

            try
            {
                _projectRepository.SaveChange();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new UpdateConflictException();
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
                    throw new UpdateConflictException();
                }
                if (project.Status != "NEW")
                {
                    throw new StatusInvalidException();
                    //return StatusCodes.Status412PreconditionFailed;
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