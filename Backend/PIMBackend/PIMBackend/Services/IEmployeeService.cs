using System.Collections.Generic;
using PIMBackend.Domain.Entities;
using PIMBackend.Domain.Objects;

namespace PIMBackend.Services
{
    /// <summary>
    ///     Example of sample service
    /// </summary>
    public interface IEmployeeService
    {
        IEnumerable<Employee> Get();

        IEnumerable<Employee> Get(Filter filter);

        Employee Get(decimal id);

        // it used to return Employee
        void Create(Employee sample);

        Employee Update(Employee sample);

        void Delete(decimal id);

        public string[] GetExistEm(string memString, bool isExist = true);
    }
}