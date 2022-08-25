using System;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using Test_Search_Api.V1.Boundary.Requests;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Test_Search_Api.V1.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;
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
            if (typeof(T) == typeof(QueryablePerson))
            {
                return (IQueryGenerator<T>) new PersonQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryablePerson>>());
            }

            if (typeof(T) == typeof(QueryableTenure))
            {
                return (IQueryGenerator<T>) new TenureQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableTenure>>());
            }

            if (typeof(T) == typeof(QueryableAsset))
            {
                return (IQueryGenerator<T>) new AssetQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAsset>>(),
                    _serviceProvider.GetService<IFilterQueryBuilder<QueryableAsset>>());
            }
            if (typeof(T) == typeof(QueryableAccount))
            {
                return (IQueryGenerator<T>) new AccountQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAccount>>());
            }

            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (IQueryGenerator<T>) new TransactionsQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableTransaction>>());
            }

            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}