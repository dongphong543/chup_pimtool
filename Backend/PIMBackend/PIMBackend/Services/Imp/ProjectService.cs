using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PIMBackend.Domain.Entities;
using PIMBackend.Domain.Objects;
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

        public IEnumerable<Project> Get(string searchText, string searchCriteria)
        {
            return _projectRepository.Get()
                                        .Include(p => p.Employees)
                                        .Where(p =>
                                                (searchText == null || searchText == "") ||
                                                ((p.ProjectNumber.ToString().Contains(searchText)) ||
                                                (p.Name.Contains(searchText)) ||
                                                (p.Customer.Contains(searchText)))
                                        )
                                        .Where(p =>
                                                (searchCriteria == null || searchCriteria == "") ||
                                                (p.Status.Contains(searchCriteria))
                                        );
        }

        public IEnumerable<Project> Get(Filter filter)
        {
            return _projectRepository.Get();
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
            return _projectRepository.Get().SingleOrDefault(x => x.ProjectNumber == pjNum);
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
            if (ProjectExistsByPjNum(project.Id) == true)
            {
                throw new IdAlreadyExistException();
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

        public Project Update(Project project)
        {
            return UpdateWithMem(project, "");
        }

        public Project UpdateWithMem(Project project, string memString)
        {
            var projectDb = _projectRepository.Get().Include(projects => projects.Employees)
                .SingleOrDefault(p => p.Id == project.Id);

            if (projectDb == null)
            {
                throw new ArgumentException();
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

                _projectRepository.UpdateWithMem(memString, projectDb);
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

        public void Delete(decimal[] ids)
        {
            for (int i = 0; i < ids.Length; ++i)
            {
                var project = _projectRepository.Get(ids[i]);
                if (project == null)
                {
                    throw new UpdateConflictException();
                }
                if (project.Status != "NEW")
                {
                    throw new StatusInvalidException();
                }
            }

            _projectRepository.Delete(ids);
            _projectRepository.SaveChange();
        }

        private bool ProjectExistsById(decimal id)
        {
            bool ret;
            try
            {
                ret = Get(id) != null;
            }
            catch (IdNotExistException)
            {
                return false;
            }

            return ret;
        }

        private bool ProjectExistsByPjNum(decimal pjNum)
        {
            bool ret;
            try
            {
                ret = GetByPjNum(pjNum) != null;
            }
            catch (ProjectNumberNotExistsException)
            {
                return false;
            }

            return ret;
        }
    }
}