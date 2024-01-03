using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id);
        /// <summary>
        /// Searches for an Employee by the given ID and loads *all* reports for that employee.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>An Employee record if one is found, null otherwise.</returns>
        Employee GetByIdRecursive(string id);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);
    }
}
