using Test_Search_Api.V1.Boundary.Requests;
using Test_Search_Api.V1.Boundary.Response;
using Test_Search_Api.V1.Gateways.Interfaces;
using Test_Search_Api.V1.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Search_Api.V1.Gateways
{
    public class SearchGateway : ISearchGateway
    {
        private readonly IElasticSearchWrapper _elasticSearchWrapper;

        public SearchGateway(IElasticSearchWrapper elasticSearchWrapper)
        {
            _elasticSearchWrapper = elasticSearchWrapper;
        }

        public async Task<GetAssetListResponse> GetListOfAssets(GetAssetListRequest query)
        {
            //what is the significance of Queryable Asset?
            var searchResponse = await _elasticSearchWrapper.Search<QueryableAsset, GetAssetListRequest>(query).ConfigureAwait(false);
            var assetListResponse = new GetAssetListResponse();

            assetListResponse.Assets.AddRange(searchResponse.Documents.Select(queryableAsset =>
                queryableAsset.Create())
            );

            assetListResponse.SetTotal(searchResponse.Total);

            return assetListResponse;
        }
    }
}
