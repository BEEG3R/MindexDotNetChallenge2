using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureTests
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

        [TestMethod]
        public async Task GetStrucutreByEmployeeId_Returns_Full_Structure_JohnLennon()
        {
            const int expectedReports = 4;
            const string johnLennonId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            HttpResponseMessage response = await _httpClient.GetAsync($"api/reportingstructure/{johnLennonId}");

            Assert.IsTrue(response.IsSuccessStatusCode);
            ReportingStructure reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReports, reportingStructure.NumberOfReports);
        }
    }
}
