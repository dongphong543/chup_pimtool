using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PIMBackend.Domain.Entities;
using PIMBackend.Errors;
using PIMBackend.Repositories;

namespace PIMBackend.Services.Imp
{
    /// <summary>
    ///     Implementation of employee service
    /// </summary>
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<Employee> Get()
        {
            return _employeeRepository.Get();
        }

        public Employee Get(decimal id)
        {
            var ret = _employeeRepository.Get().SingleOrDefault(x => x.Id == id);

            return ret;
        }

        // it type used to be Employee
        public void Create(Employee employee)
        {
            if (Get(employee.Id) != null)
            {
                throw new IdAlreadyExistException("Employee exists: ", employee.Id);
            }

            else
            {
                employee.Version = 0;
                _employeeRepository.Add(employee);

                try
                {
                    _employeeRepository.SaveChange();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new UpdateConflictException("Conflict in create.", e);
                }
            }
        }

        public Employee Update(Employee employee)
        {
            var employeeDb = _employeeRepository.Get(employee.Id);
            if (employeeDb == null)
            {
                throw new IdNotExistException();
            }

            if (employee.Version != employeeDb.Version)
            {
                throw new UpdateConflictException("Conflict in update.", null);
            }

            else
            {
                employeeDb.Visa = employee.Visa;
                employeeDb.FirstName = employee.FirstName;
                employeeDb.LastName = employee.LastName;
                employeeDb.BirthDate = employee.BirthDate;
                employeeDb.Version += 1;
            }

            try
            {
                _employeeRepository.SaveChange();
            }
            catch (DbUpdateConcurrencyException e) 
            {
                throw new UpdateConflictException("Conflict in update.", e);
            }
            
            return employeeDb;
        }

        public void Delete(decimal id)
        {
            _employeeRepository.Delete(id);
            _employeeRepository.SaveChange();
        }

        public string[] GetExistOrNotExistEmployeeArray(string memString, bool isExist = true)
        {
            return _employeeRepository.GetExistOrNotExistEmployeeArray(memString, isExist);
        }
    }
}