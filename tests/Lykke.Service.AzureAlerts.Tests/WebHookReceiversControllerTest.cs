using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Lykke.Service.AzureAlerts.Tests
{
    public class WebHookReceiversControllerTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public WebHookReceiversControllerTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [Fact]
        public async void SuccessfulPath()
        {


            var str = @"{
  ""status"": ""Active"",
                      ""context"": {
                        ""id"": ""string"",
                        ""name"": ""string"",
                        ""description"": ""string"",
                        ""conditionType"": ""string"",
                        ""subscriptionId"": ""string"",
                        ""timestamp"": ""2017-09-01T21:40:12.057Z"",
                        ""condition"": {
                          ""metricName"": ""Free memory"",
                          ""metricUnit"": ""Percent"",
                          ""metricValue"": ""20"",
                          ""threshold"": ""10"",
                          ""windowSize"": ""string"",
                          ""timeAggregation"": ""string"",
                          ""operator"": ""string""
                        },
                        ""resourceGroupName"": ""string"",
                        ""resourceName"": ""string"",
                        ""resourceType"": ""string"",
                        ""resourceId"": ""string"",
                        ""resourceRegion"": ""string"",
                        ""portalLink"": ""string""
                      },
                        ""properties"": {}
            }";
            var content = new StringContent(str, Encoding.UTF8, "application/json");
            var response1 = await _client.PostAsync(@"api/webhooks/incoming?code=CB26AFC1-A31E-44EC-BC48-8E79BF2C90D7", content);
            response1.EnsureSuccessStatusCode();

        }
    }

}
