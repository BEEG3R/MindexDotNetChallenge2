using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        private static Task<HttpResponseMessage> CreateTestCompensationRecord()
        {

            DateTime expectedEffectiveDate = DateTime.Today;
            const double expectedSalary = 1000.00;
            var compensation = new Compensation()
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                EffectiveDate = expectedEffectiveDate,
                Salary = expectedSalary
            };
            return _httpClient.PostAsJsonAsync($"api/compensation", compensation);
        }

        [TestMethod]
        public async Task CreateCompensation_Returns_Created()
        {
            HttpResponseMessage response = await CreateTestCompensationRecord();

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(DateTime.Today, newCompensation.EffectiveDate);
            Assert.AreEqual(1000, newCompensation.Salary);
        }

        [TestMethod]
        public async Task CreateCompensation_Returns_BadRequest_If_EmployeeId_Does_Not_Exist()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "invalid",
                EffectiveDate = DateTime.Today,
                Salary = -1
            };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/compensation", compensation);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetByEmployeeId_Returns_Not_Found_If_No_Records_Exist()
        {
            const string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            HttpResponseMessage response = await _httpClient.GetAsync($"api/compensation/{employeeId}");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetByEmployeeId_Returns_Ok_If_Record_Exists()
        {
            // we need to create the record before trying to retrieve it
            var test = await CreateTestCompensationRecord();

            const string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            HttpResponseMessage response = await _httpClient.GetAsync($"api/compensation/{employeeId}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensations = response.DeserializeContent<IEnumerable<Compensation>>();
            Assert.IsTrue(compensations.Count() == 1);
            Assert.AreEqual(DateTime.Today, compensations.First().EffectiveDate);
            Assert.AreEqual(1000, compensations.First().Salary);
        }
    }
}
