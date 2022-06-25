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

        //public EmployeeController(PIMContext context)
        //{
        //    _context = context;
        //}

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

            return _employeeService.GetExistEm(MemStringDTO.VisaStr, false);

        }

        [HttpPost("exist")]
        public string[] GetExistEmployee(MemStringDTO MemStringDTO)
        {

            //return await GetExistEm(MemStringDTO, true);
            //return _employeeService.GetExistEm(MemStringDTO.VisaStr);
            return _employeeService.GetExistEm(MemStringDTO.VisaStr, true);

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


        //public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        //{
        //    return await _context.Employees.ToListAsync();
        //}




        //public async Task<ActionResult<Employee>> GetEmployee(decimal id)
        //{
        //    var employee = await _context.Employees.FindAsync(id);

        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return employee;
        //}







        //public async Task<IActionResult> PutEmployee(int id, Employee Employee)
        //{
        //    if (id != Employee.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(Employee).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EmployeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}








        //public async Task<ActionResult<Employee>> PostEmployee(Employee Employee)
        //{
        //    _context.Employees.Add(Employee);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmployee", new { id = Employee.Id }, Employee);
        //}




        //public async Task<IActionResult> DeleteEmployee(int id)
        //{
        //    var Employee = await _context.Employees.FindAsync(id);
        //    if (Employee == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Employees.Remove(Employee);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EmployeeExists(int id)
        //{
        //    return _context.Employees.Any(e => e.Id == id);

        //}

        //protected bool EmployeeExistsVisa(string visa)
        //{
        //    return _context.Employees.Any(e => e.Visa == visa);
        //}

        //protected internal async Task<string[]> GetExistEm(MemStringDTO memStringDTO, bool isExist = true)
        //{
        //    string VisaStr = memStringDTO.VisaStr;

        //    List<string> ret = new List<string>();
        //    List<string> visas = new List<string>();

        //    if (VisaStr != null && VisaStr.Length > 0)
        //    {
        //        visas = VisaStr.Split(",").ToList<string>();

        //        for (int i = 0; i < visas.Count; ++i)
        //        {
        //            visas[i] = visas[i].Trim();
        //        }

        //    }

        //    visas = visas.Where(x => x != "").ToList();

        //    for (int i = 0; i < visas.Count; ++i)
        //    {
        //        //var employee = await _context.Employees.FindAsync(Visas[i]);
        //        List<Employee> employeeVisaLst = new List<Employee>();

        //        employeeVisaLst = await _context.Employees.Where(x => x.Visa.ToUpper() == visas[i]).ToListAsync();

        //        if (employeeVisaLst.Count > 0 == isExist)
        //        {
        //            ret.Add(visas[i].ToUpper());
        //        }


        //    }

        //    return ret.ToArray();
        //}
    }
}