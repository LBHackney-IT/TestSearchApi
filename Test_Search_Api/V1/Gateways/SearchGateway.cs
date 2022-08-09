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

        [LogCall]
        public async Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryablePerson, GetPersonListRequest>(query).ConfigureAwait(false);
            var personListResponse = new GetPersonListResponse();

            personListResponse.Persons.AddRange(searchResponse.Documents.Select(queryablePerson =>
                queryablePerson.Create())
            );

            personListResponse.SetTotal(searchResponse.Total);

            return personListResponse;
        }

        [LogCall]
        public async Task<GetTenureListResponse> GetListOfTenures(GetTenureListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableTenure, GetTenureListRequest>(query).ConfigureAwait(false);
            var tenureListResponse = new GetTenureListResponse();

            tenureListResponse.Tenures.AddRange(searchResponse.Documents.Select(queryableTenure =>
                queryableTenure.Create())
            );

            tenureListResponse.SetTotal(searchResponse.Total);

            return tenureListResponse;
        }

        [LogCall]
        public async Task<GetAssetListResponse> GetListOfAssets(GetAssetListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableAsset, GetAssetListRequest>(query).ConfigureAwait(false);
            var assetListResponse = new GetAssetListResponse();

            assetListResponse.Assets.AddRange(searchResponse.Documents.Select(queryableAsset =>
                queryableAsset.Create())
            );

            assetListResponse.SetTotal(searchResponse.Total);

            return assetListResponse;
        }

        [LogCall]
        public async Task<GetAllAssetListResponse> GetListOfAssetsSets(GetAllAssetListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.SearchSets<QueryableAsset, GetAllAssetListRequest>(query).ConfigureAwait(false);
            var assetListResponse = new GetAllAssetListResponse();

            if (searchResponse == null) return assetListResponse;
            assetListResponse.Assets.AddRange(searchResponse.Documents.Select(queryableAsset =>
                queryableAsset.CreateAll())
            );

            assetListResponse.SetTotal(searchResponse.Total);
            if (searchResponse.Documents.Count > 0)
            {
                assetListResponse.SetLastHitId(searchResponse.Hits.Last().Id);
            }

            return assetListResponse;
        }

        public async Task<GetAccountListResponse> GetListOfAccounts(GetAccountListRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableAccount, GetAccountListRequest>(query).ConfigureAwait(false);
            var accountListResponse = GetAccountListResponse.Create(searchResponse.Documents.Select(queryableAccount =>
                queryableAccount.ToAccount())?.ToList());

            accountListResponse.SetTotal(searchResponse.Total);

            return accountListResponse;
        }

        public async Task<GetTransactionListResponse> GetListOfTransactions(GetTransactionListRequest request)
        {
            var searchRequest = new GetTransactionListRequest
            {
                SearchText = request.SearchText,
                Page = request.Page,
                PageSize = request.PageSize,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            var searchResponse = await _elasticSearchWrapper.Search<QueryableTransaction, GetTransactionListRequest>(searchRequest).ConfigureAwait(false);

            if (searchResponse == null) throw new Exception("Cannot get response from ElasticSearch instance");

            if (!searchResponse.IsValid) throw new Exception($"Cannot load transactions list. Error: {searchResponse.ServerError}");

            if (searchResponse.Documents == null) throw new Exception($"ElasticSearch instance returns no documents. Error: {searchResponse.ServerError}");

            var transactions = searchResponse.Documents.Select(queryableTransaction => queryableTransaction.ToTransaction());

            return GetTransactionListResponse.Create(searchResponse.Total, transactions.ToResponse());
        }
    }
}
