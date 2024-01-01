using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
