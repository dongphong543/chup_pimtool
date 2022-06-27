using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PIMBackend.Database;
using PIMBackend.Domain.Entities;
using PIMBackend.DTOs;
using PIMBackend.Errors;
using PIMBackend.Repositories;
using PIMBackend.Services;

namespace PIMBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        // GET: api/Project
        [HttpGet]
        public IEnumerable<ProjectDTO> Get(string searchText, string searchCriteria)
        {
            return _mapper  .Map<IEnumerable<Project>, IEnumerable<ProjectDTO>>(_projectService
                            .Get(searchText, searchCriteria))
                            .OrderBy(p => p.ProjectNumber);
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public ProjectDTO Get(decimal id)
        {
            return _mapper.Map<Project, ProjectDTO>(_projectService.Get(id));
        }


        [HttpGet("pjNum/{pjNum}")]
        public ProjectDTO GetByPjNum(decimal pjNum)
        {
            return _mapper.Map<Project, ProjectDTO>(_projectService.GetByPjNum(pjNum));
        }

        [HttpPost("exist")]
        public bool ProjectNumbersExist(decimal[] pjNums)
        {
            
            for (int i = 0; i < pjNums.Length; ++i)
            {
                if (_projectService.ProjectNumberExists(pjNums[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        [HttpPost("deletable")]
        public bool ProjectNumbersDeletable(decimal[] pjNums)
        {
            ProjectDTO project = null;

            for (int i = 0; i < pjNums.Length; ++i)
            {
                project = GetByPjNum(pjNums[i]);
            

                if (project.Status != "NEW")
                {
                    return false;
                }
            }

            if (project == null)
            {
                return false;
            }

            return true;
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public ProjectDTO PutProject(ProjectAddDTO obj)
        {
            return _mapper.Map<Project, ProjectDTO>(_projectService.Update(_mapper.Map<ProjectDTO, Project>(obj.project), obj.memString));
        }

        // POST: api/Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        public void Post(ProjectAddDTO obj)
        {
            if (string.IsNullOrEmpty(obj.memString))
            {
                _projectService.Create(_mapper.Map<ProjectDTO, Project>(obj.project));
            }

            else
            {
                _projectService.CreateWithMem(_mapper.Map<ProjectDTO, Project>(obj.project), obj.memString);
            }
            
            // if error throw exception
        }

        // DELETE: api/Employee/5
        [HttpDelete]
        public void Delete(decimal[] pjNums)
        {
            _projectService.Delete(pjNums);
        }
    }
}