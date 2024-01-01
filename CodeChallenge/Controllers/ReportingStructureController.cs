using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reportingstructure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "getNumberOfReportsByEmployeeId")]
        public IActionResult GetStructureByEmployeeId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("You must provide an Employee ID to obtain their reporting structure.");
            }
            _logger.LogDebug($"Received reporting structure GET request for Employee ID '{id}'");

            Employee employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound($"No Employee record was found for ID '{id}'");
            }

            ReportingStructure rs = new(employee);
            return Ok(rs);
        }
    }
}
