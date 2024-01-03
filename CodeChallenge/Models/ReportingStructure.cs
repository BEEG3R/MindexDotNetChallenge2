using System;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException("Employee cannot be null");
            }
            this.Employee = employee;
        }

        public Employee Employee { get; }
        public int NumberOfReports
        {
            get
            {
                return GetAllReports(Employee);
            }
        }

        /// <summary>
        /// Traverse the Direct Reports tree using the given Employee as the root.
        /// Direct Reports of Direct Reports are counted in the total produced by this method.
        /// </summary>
        /// <param name="employee">The Employee to use as the root of the lookup tree.</param>
        /// <returns>An integer representing the number of Employees who report to the given Employee.</returns>
        private static int GetAllReports(Employee employee)
        {
            if (employee == null || employee.DirectReports == null)
            {
                return 0;
            }
            int sum = 0;
            foreach (Employee e in employee.DirectReports)
            {
                sum++;
                sum += GetAllReports(e);
            }
            return sum;
        }
    }
}
