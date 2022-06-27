using System.Collections.Generic;
using PIMBackend.Domain.Entities;

namespace PIMBackend.Services
{
    /// <summary>
    ///     Example of sample service
    /// </summary>
    public interface IEmployeeService
    {
        IEnumerable<Employee> Get();

        Employee Get(decimal id);

        // it used to return Employee
        void Create(Employee sample);

        Employee Update(Employee sample);

        void Delete(decimal id);

        public string[] GetExistOrNotExistEmployeeArray(string memString, bool isExist = true);
    }
}