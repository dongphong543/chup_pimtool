using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMBackend.Database;
using PIMBackend.Domain.Entities;
using PIMBackend.DTOs;
using Microsoft.VisualBasic;
using AutoMapper;
using PIMBackend.Services;

namespace PIMBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        //public GroupController(PIMContext context)
        //{
        //    _context = context;
        //}

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        // GET: api/Group
        [HttpGet]
        public IEnumerable<GroupDTO> Get()
        {
            return _mapper.Map<IEnumerable<Group>, IEnumerable<GroupDTO>>(_groupService.Get());
        }

        // GET: api/Group/5
        [HttpGet("{id}")]
        public GroupDTO Get(decimal id)
        {
            return _mapper.Map<Group, GroupDTO>(_groupService.Get(id));
        }

        // PUT: api/Group/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public GroupDTO Put(GroupDTO group)
        {
            return _mapper.Map<Group, GroupDTO>(_groupService.Update(_mapper.Map<GroupDTO, Group>(group)));
        }

        // POST: api/Group
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void Post(GroupDTO group)
        {
            _groupService.Create(_mapper.Map<GroupDTO, Group>(group));
            // if error throw exception
        }

        // DELETE: api/Group/5
        [HttpDelete("{id}")]
        public void Delete(decimal id)
        {
            _groupService.Delete(id);
        }
    }
}