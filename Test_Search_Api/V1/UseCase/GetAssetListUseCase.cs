using System.Threading.Tasks;
using Test_Search_Api.V1.Boundary.Request;
using Test_Search_Api.V1.Boundary.Response;
using Test_Search_Api.V1.Gateways.Interfaces;
using Test_Search_Api.V1.UseCase.Interfaces;

namespace Test_Search_Api.V1.UseCase
{
    public class GetAssetListUseCase : IGetAssetListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetAssetListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        public async Task<GetAssetListResponse> ExecuteAsync(GetAssetListRequest housingSearchRequest)
        {
            return await _searchGateway.GetListOfAssets(housingSearchRequest).ConfigureAwait(false);
        }
    }
}