using FluentAssertions;
using Hackney.Core.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Test_Search_Api.Tests.V1.E2ETests.Steps
{
    public class HealthCheckSteps : BaseSteps
    {
        public HealthCheckSteps(HttpClient httpClient) : base(httpClient)
        { }

        private async Task<HttpResponseMessage> CallApi()
        {
            var route = $"api/v1/healthcheck/ping";
            var uri = new Uri(route, UriKind.Relative);
            return await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        private async Task<HealthCheckResponse> ExtractResultFromHttpResponse(HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResult = JsonSerializer.Deserialize<HealthCheckResponse>(responseContent, _jsonOptions);
            return apiResult;
        }

        public async Task WhenTheHealtchCheckIsCalled()
        {
            _lastResponse = await CallApi().ConfigureAwait(false);
        }

        public async Task ThenTheHealthyStatusIsReturned()
        {
            var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);
            apiResult.Status.Should().Be(HealthStatus.Healthy);
            apiResult.Entries.First().Key.Should().Be("Elastic search");
            apiResult.Entries["Elastic search"].Status.Should().Be(HealthStatus.Healthy);
        }
    }
}

