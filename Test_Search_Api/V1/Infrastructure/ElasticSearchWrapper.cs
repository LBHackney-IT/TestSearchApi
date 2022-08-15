using Test_Search_Api.V1.Boundary.Requests;
using Test_Search_Api.V1.Interfaces.Sorting;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Test_Search_Api.V1.Interfaces;
using Test_Search_Api.V1.Interfaces.Factories;
using Test_Search_Api.V1.Interfaces.Filtering;

namespace HousingSearchApi.V1.Infrastructure
{
    public class ElasticSearchWrapper : IElasticSearchWrapper
    {
        private readonly IElasticClient _esClient;
        private readonly IQueryFactory _queryFactory;
        private readonly IPagingHelper _pagingHelper;
        private readonly ISortFactory _sortFactory;
        private readonly IFilterFactory _filterFactory;
        private readonly IIndexSelector _indexSelector;

        public ElasticSearchWrapper(IElasticClient esClient, IQueryFactory queryFactory,
            IPagingHelper pagingHelper, ISortFactory sortFactory, ILogger<ElasticSearchWrapper> logger, IIndexSelector indexSelector,
            IFilterFactory filterFactory)
        {
            _esClient = esClient;
            _queryFactory = queryFactory;
            _pagingHelper = pagingHelper;
            _sortFactory = sortFactory;
            _filterFactory = filterFactory;
            _logger = logger;
            _indexSelector = indexSelector;
        }

        public async Task<ISearchResponse<T>> Search<T, TRequest>(TRequest request) where T : class where TRequest : class
        {
            try
            {
                var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));

                if (request == null)
                    return new SearchResponse<T>();

                HousingSearchRequest searchRequest = (HousingSearchRequest) (object) request;

                var pageOffset = _pagingHelper.GetPageOffset(searchRequest.PageSize, searchRequest.Page);

                var result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                    .Query(q => BaseQuery<T>().Create(request, q))
                    .PostFilter(q => _filterFactory.Create<T, TRequest>(request).GetDescriptor(q, request))
                    .Sort(_sortFactory.Create<T, TRequest>(request).GetSortDescriptor)
                    .Size(searchRequest.PageSize)
                    .Skip(pageOffset)
                    .TrackTotalHits()).ConfigureAwait(false);

                return result;
            }
            catch (ElasticsearchClientException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ISearchResponse<T>> SearchSets<T, TRequest>(TRequest request) where T : class where TRequest : class
        {
            var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));

            if (request == null)
                return new SearchResponse<T>();
            var searchRequest = (GetAllAssetListRequest) (object) request;

            var elements = !string.IsNullOrEmpty(searchRequest.LastHitId) ? new string[] { searchRequest.LastHitId } : new string[] { string.Empty };
            var lastSortedItem = !string.IsNullOrEmpty(searchRequest.LastHitId) ? elements.Cast<object>().ToArray() : null;

            ISearchResponse<T> result = null;

            try
            {
                if (string.IsNullOrEmpty(searchRequest.LastHitId) && searchRequest.Page == 1)
                {
                    result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                      .Query(q => BaseQuery<T>().Create(request, q))
                      .Size(searchRequest.PageSize)
                      .Sort(_sortFactory.Create<T, TRequest>(request).GetSortDescriptor)
                      .TrackTotalHits()
                      ).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(searchRequest.LastHitId))
                {
                    result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                      .Query(q => BaseQuery<T>().Create(request, q))
                      .Size(searchRequest.PageSize)
                      .TrackTotalHits()
                      .SearchAfter(lastSortedItem)
                      .Sort(_sortFactory.Create<T, TRequest>(request).GetSortDescriptor)
                      ).ConfigureAwait(false);
                }

                return result;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private IQueryGenerator<T> BaseQuery<T>() where T : class
        {
            return _queryFactory.CreateQuery<T>();
        }
    }
}
