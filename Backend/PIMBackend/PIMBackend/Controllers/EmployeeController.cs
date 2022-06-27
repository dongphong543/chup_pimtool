using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMBackend.Database;
using PIMBackend.Domain.Entities;
using PIMBackend.DTOs;
using PIMBackend.Services;

namespace PIMBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // GET: api/Employee
        [HttpGet]
        public IEnumerable<EmployeeDTO> Get()
        {
            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDTO>>(_employeeService.Get());
        }
        // GET: api/Employee/5
        [HttpGet("{id}")]
        public EmployeeDTO Get(decimal id)
        {
            return _mapper.Map<Employee, EmployeeDTO>(_employeeService.Get(id));
        }

        [HttpPost("nonexist")]
        public string[] GetNonExistEmployee(MemStringDTO MemStringDTO)
        {

            return _employeeService.GetExistOrNotExistEmployeeArray(MemStringDTO.VisaStr, false);

        }

        [HttpPost("exist")]
        public string[] GetExistEmployee(MemStringDTO MemStringDTO)
        {
            return _employeeService.GetExistOrNotExistEmployeeArray(MemStringDTO.VisaStr, true);
        }


        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public EmployeeDTO Put(EmployeeDTO employee)
        {
            return _mapper.Map<Employee, EmployeeDTO>(_employeeService.Update(_mapper.Map<EmployeeDTO, Employee>(employee)));
        }


        // POST: api/Employee
        [HttpPost]
        public void Post(EmployeeDTO employee)
        {
            _employeeService.Create(_mapper.Map<EmployeeDTO, Employee>(employee));
            // if error throw exception
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public void Delete(decimal id)
        {
            _employeeService.Delete(id);
        }

    }
}