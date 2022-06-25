﻿using PIMBackend.Domain.Entities;

namespace PIMBackend.Repositories
{
    /// <summary>
    ///     Example repository interface
    /// </summary>
    public interface IEmployeeRepository : IRepository<Employee>
    {
        public Employee GetByVisa(string visa);
        string[] GetExistEm(string memString, bool isExist);
    }
}