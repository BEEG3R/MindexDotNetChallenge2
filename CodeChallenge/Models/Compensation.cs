using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public double Salary { get; set; }
        public DateOnly EffectiveDate { get; set; }
    }
}
