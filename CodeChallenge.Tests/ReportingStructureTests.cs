using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Http;
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
            ReportingStructure rs = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReports, rs.NumberOfReports);
        }

        [TestMethod]
        public async Task GetStructureByEmployeeId_Returns_No_Reports_PeteBest()
        {
            const int expectedReports = 0;
            const string peteBestId = "62c1084e-6e34-4630-93fd-9153afb65309";

            HttpResponseMessage response = await _httpClient.GetAsync($"api/reportingstructure/{peteBestId}");

            Assert.IsTrue(response.IsSuccessStatusCode);
            ReportingStructure rs = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReports, rs.NumberOfReports);
        }

        [TestMethod]
        public async Task GetStructureByEmployeeId_Returns_NotFound_For_Null_ID()
        {
            const string badId = null;

            HttpResponseMessage response = await _httpClient.GetAsync($"api/reportingstructure/{badId}");

            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetStructureByEmployeeId_Returns_NotFound_For_Invalid_ID()
        {
            const string badId = "-1";

            HttpResponseMessage response = await _httpClient.GetAsync($"api/reportingstructure/{badId}");

            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
