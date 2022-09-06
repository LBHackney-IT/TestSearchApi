using System;
using Test_Search_Api.V1.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;
using Test_Search_Api.V1.Gateways;
using Hackney.Core.ElasticSearch.Interfaces;
using Test_Search_Api.V1.Interfaces;

namespace Test_Search_Api.V1.Infrastructure.Factories
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IQueryGenerator<T> CreateQuery<T>() where T : class
        {

            if (typeof(T) == typeof(QueryableAsset))
            {
                return (IQueryGenerator<T>) new AssetQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAsset>>(),
                    _serviceProvider.GetService<IFilterQueryBuilder<QueryableAsset>>());
            }

            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}