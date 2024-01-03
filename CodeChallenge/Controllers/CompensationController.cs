using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, 
            IEmployeeService employeeService,
            ICompensationService compensationService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _compensationService = compensationService;
        }

        /// <summary>
        /// Create a new compensation record
        /// </summary>
        /// <param name="compensation">Compensation record to save</param>
        /// <returns>Saved Compensation record</returns>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received request to create new compensation for employee ID '{compensation.EmployeeId}'");

            var employee = _employeeService.GetById(compensation.EmployeeId);
            if (employee == null)
            {
                return BadRequest($"Could not find an employee by id '{compensation.EmployeeId}' to create the compensation record for");
            }

            _compensationService.Create(compensation);

            return CreatedAtRoute("getByEmployeeId", new {id = compensation.EmployeeId}, compensation);
        }

        /// <summary>
        /// Get all Compensation records for the given employee id
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Collection of Compensation records</returns>
        [HttpGet("{id}", Name = "getByEmployeeId")]
        public IActionResult GetByEmployeeId(string id)
        {
            _logger.LogDebug($"Received compensation get request for employee id '{id}'");
            var compensation = _compensationService.GetByEmployeeID(id);
            if (compensation == null || !compensation.Any())
            {
                return NotFound();
            }
            return Ok(compensation);
        }
    }
}
