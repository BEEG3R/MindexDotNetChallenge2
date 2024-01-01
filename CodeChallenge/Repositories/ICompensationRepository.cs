using CodeChallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        IEnumerable<Compensation> GetByEmployeeId(string id);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}
