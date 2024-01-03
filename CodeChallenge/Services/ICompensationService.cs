using CodeChallenge.Models;
using System.Collections.Generic;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        IEnumerable<Compensation> GetByEmployeeID(string id);
        Compensation Create(Compensation compensation);
    }
}
