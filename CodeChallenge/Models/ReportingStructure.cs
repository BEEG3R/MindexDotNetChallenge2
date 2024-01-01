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
