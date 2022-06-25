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

        public IEnumerable<Employee> Get(Filter filter)
        {
            return _employeeRepository.Get();
        }

        public Employee Get(decimal id)
        {
            var ret = _employeeRepository.Get().SingleOrDefault(x => x.Id == id);
            //if (ret == null)
            //{
            //    throw new IdNotExistException();
            //}

            return ret;
        }

        // it type used to be Employee
        public void Create(Employee employee)
        {
            if (Get(employee.Id) != null)
            {
                throw new IdAlreadyExistException();
            }

            else
            {
                employee.Version = 0;
                _employeeRepository.Add(employee);

                try
                {
                    _employeeRepository.SaveChange();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new UpdateConflictException();
                }
            }
        }

        public Employee Update(Employee employee)
        {
            var employeeDb = _employeeRepository.Get(employee.Id);
            if (employeeDb == null)
            {
                throw new ArgumentException();
            }

            if (employee.Version != employeeDb.Version)
            {
                throw new UpdateConflictException();
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
            catch (DbUpdateConcurrencyException) 
            {
                throw new UpdateConflictException();
            }
            
            return employeeDb;
        }

        public void Delete(decimal id)
        {
            _employeeRepository.Delete(id);
            _employeeRepository.SaveChange();
        }

        public string[] GetExistEm(string memString, bool isExist = true)
        {
            return _employeeRepository.GetExistEm(memString, isExist);
        }
    }
}