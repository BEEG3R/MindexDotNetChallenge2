using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ILogger<ICompensationService> _logger;
        private readonly ICompensationRepository _compensationRepo;

        public CompensationService(ILogger<ICompensationService> logger, ICompensationRepository compensationRepo)
        {
            _logger = logger;
            _compensationRepo = compensationRepo;
        }

        public Compensation Create(Compensation compensation)
        {
            if (compensation == null)
            {
                return null;
            }
            _compensationRepo.Add(compensation);
            _compensationRepo.SaveAsync().Wait();
            return compensation;
        }

        public IEnumerable<Compensation> GetByEmployeeID(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return _compensationRepo.GetByEmployeeId(id);
        }
    }
}
