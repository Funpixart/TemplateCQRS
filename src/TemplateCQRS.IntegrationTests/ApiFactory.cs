using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using TemplateCQRS.Api;
using TemplateCQRS.Domain.Common;
using Xunit;

namespace TemplateCQRS.IntegrationTests
{
    public class ApiFactory
    {
        [Fact]
        public async Task Default()
        {
            var appFactory = new WebApplicationFactory<Program>();
            var httpClient = appFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync(ApiRoutes.Claim);
            var result = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(response.StatusCode, HttpStatusCode.NoContent);
        }
    }
}
