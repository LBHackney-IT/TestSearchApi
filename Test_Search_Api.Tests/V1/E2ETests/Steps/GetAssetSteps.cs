using FluentAssertions;
using Hackney.Core.Testing.Shared.E2E;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Test_Search_Api.Tests.V1.E2ETests.Fixtures;
using Test_Search_Api.V1.Boundary.Response;
using Test_Search_Api.V1.Boundary.Responses.Metadata;
using BaseSteps = HousingSearchApi.Tests.V1.E2ETests.Steps.Base.BaseSteps;

namespace Test_Search_Api.Tests.V1.E2ETests.Steps
{
    public class GetAssetSteps : BaseSteps
    {
        private string _lastHitId;
        public GetAssetSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/assets", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/assets?searchText=%20abc", UriKind.Relative)).ConfigureAwait(false);
        }


        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/assets?searchText={AssetFixture.Addresses.Last().FirstLine}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenAssetTypesAreProvided(string assetType)
        {
            var route = new Uri($"api/v1/search/assets?searchText={AssetFixture.Addresses.Last()}&assetTypes={assetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }
        public async Task WhenSearchTextProvidedAsStarStarAndAssetTypeProvidedAndLastHitIdNotProvided(string assetType)
        {
            var route = new Uri($"api/v1/search/assets/all?searchText=**&assetTypes={assetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }
        public async Task WhenSearchTextProvidedAsStarStarAndAssetTypeProvidedAndLastHitIdProvided(string assetType)
        {
            var route = new Uri($"api/v1/search/assets/all?searchText=**&assetTypes={assetType}&pageSize={5}&lastHitId={_lastHitId}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }
        public async Task WhenAnAssetStatusIsProvided(string assetStatus)
        {
            var route = new Uri($"api/v1/search/assets/all?assetStatus={assetStatus}&pageSize={1}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenNoOfBedSpacesIsProvided(int numberOfBedSpaces)
        {
            var route = new Uri($"api/v1/search/assets/all?numberOfBedSpaces={numberOfBedSpaces}&pageSize={1}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenFloorNoIsProvided(int floorNo)
        {
            var route = new Uri($"api/v1/search/assets/all?floorNo={floorNo}&pageSize={1}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenAnExactMatchExists(string address)
        {
            var route = new Uri($"api/v1/search/assets?searchText={address}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            result.Results.Assets.Count.Should().Be(pageSize);
        }

        public async Task ThenOnlyTheseAssetTypesShouldBeIncluded(string allowedAssetType)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            var assets = allowedAssetType.Split(",");

            result.Results.Assets.All(x => x.AssetType == assets[0] || x.AssetType == assets[1]);
        }
        public async Task ThenOnlyAllAssetsResponseTheseAssetTypesShouldBeIncluded(string allowedAssetType)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIAllResponse<GetAllAssetListResponse>>(resultBody, _jsonOptions);

            var assets = allowedAssetType.Split(",");

            result.Results.Assets.All(x => x.AssetType == assets[0] || x.AssetType == assets[1]);

        }
        public async Task ThenOnlyLastHitIdShouldBeIncluded()
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIAllResponse<GetAllAssetListResponse>>(resultBody, _jsonOptions);

            _lastHitId = result?.LastHitId;
        }

        public async Task ThenThatAddressShouldBeTheFirstResult(string address)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            result.Results.Assets.First().AssetAddress.AddressLine1.Should().Be(address);
        }

        public async Task ThenOnlyAllAssetsResponseTheseAssetStatusesShouldBeIncluded(string allowedAssetStatus)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIAllResponse<GetAllAssetListResponse>>(resultBody, _jsonOptions);

            var assets = allowedAssetStatus.Split(",");

            result.Results.Assets.All(x => x.AssetStatus == assets[0] || x.AssetStatus == assets[1]);
        }

        public async Task ThenNumberOfBedSpacesShouldBeInResult(int numberOfBedSpaces)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            result.Results.Assets.All(x => x.AssetCharacteristics.NumberOfBedSpaces == numberOfBedSpaces);
        }
        public async Task ThenFloorNoShouldBeInResult(int floorNo)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            result.Results.Assets.All(x => x.AssetLocation.FloorNo == floorNo);
        }
    }
}
