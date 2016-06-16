using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationManager.Client.Tests
{
    [TestClass]
    public class ConfigurationClient_IntegrationTest
    {
        private ConfigurationClient _client;

        [TestInitialize]
        public void InitConfigurationClient()
        {
            var url = System.Configuration.ConfigurationManager.ConnectionStrings["ConfigurationManager.WebService"].ConnectionString;
            _client = new ConfigurationClient(url);
        }

        [TestMethod]
        public async Task GetAsync_Should_Match_SetAsync_Test()
        {
            string key = "Client/IntegrationTest/Key";
            string value = "Client - IntegrationTest - Value";

            await _client.SetAsync(key, value);
            var returnedValue = await _client.GetAsync(key);

            Assert.AreEqual(value, returnedValue);
        }

        [ExpectedException(typeof(ConfigurationServiceException))]
        public async Task SetKey_With_Invalid_Key_Should_ThrowException()
        {
            string key = "Client/IntegrationTest/Key_";
            string value = "Client - IntegrationTest - Value";

            await _client.SetAsync(key, value);
        }
    }
}
