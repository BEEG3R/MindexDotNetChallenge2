using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees
                .SingleOrDefault(e => e.EmployeeId == id);
        }

        public Employee GetByIdRecursive(string id, Employee e = null)
        {
            Employee result = new();
            Employee lookup = _employeeContext.Employees
                    .Include(e => e.DirectReports)
                    .SingleOrDefault(e => e.EmployeeId == id);
            if (lookup == null)
            {
                return null;
            }

            // if e is null, we haven't started to recurse.
            // populate the Employee object with all properties from the lookup, then start the recursion.
            if (e == null)
            {
                result = new Employee()
                {
                    EmployeeId = lookup.EmployeeId,
                    FirstName = lookup.FirstName,
                    LastName = lookup.LastName,
                    Department = lookup.Department,
                    DirectReports = lookup.DirectReports,
                    Position = lookup.Position
                };
                foreach (Employee emp in lookup.DirectReports)
                {
                    GetByIdRecursive(emp.EmployeeId, emp);
                }
                return result;
            }

            // if we reach here, this is from a recursive call.
            // populate the Direct Reports property
            result.DirectReports = new List<Employee>
            {
                new Employee()
                {
                    EmployeeId = lookup.EmployeeId,
                    FirstName = lookup.FirstName,
                    LastName = lookup.LastName,
                    Department = lookup.Department,
                    Position = lookup.Position
                }
            };
            foreach (Employee emp in lookup.DirectReports)
            {
                GetByIdRecursive(emp.EmployeeId, emp);
            }
            return result;

        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
