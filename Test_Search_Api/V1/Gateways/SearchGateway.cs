using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using Test_Search_Api.V1.Boundary.Requests;
using Test_Search_Api.V1.Boundary.Responses;
using Test_Search_Api.V1.Boundary.Responses.Transactions;
using Test_Search_Api.V1.Factories;
using Test_Search_Api.V1.Gateways.Interfaces;
using Test_Search_Api.V1.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

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
