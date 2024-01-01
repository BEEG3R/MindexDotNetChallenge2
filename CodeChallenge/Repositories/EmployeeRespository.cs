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
            if (e == null)
            {
                Employee root = _employeeContext.Employees
                    .Include(e => e.DirectReports)
                    .SingleOrDefault(e => e.EmployeeId == id);
                result = new Employee()
                {
                    EmployeeId = root.EmployeeId,
                    FirstName = root.FirstName,
                    LastName = root.LastName,
                    Department = root.Department,
                    DirectReports = root.DirectReports,
                    Position = root.Position
                };
                foreach (Employee emp in root.DirectReports)
                {
                    GetByIdRecursive(emp.EmployeeId, emp);
                }
                return result;
            }
            Employee report = _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == id);
            if (report == null)
            {
                return null;
            }
            result.DirectReports = new List<Employee>
            {
                new Employee()
                {
                    EmployeeId = report.EmployeeId,
                    FirstName = report.FirstName,
                    LastName = report.LastName,
                    Department = report.Department,
                    Position = report.Position
                }
            };
            foreach (Employee emp in report.DirectReports)
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
