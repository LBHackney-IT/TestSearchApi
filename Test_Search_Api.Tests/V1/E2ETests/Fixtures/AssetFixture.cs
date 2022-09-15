using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Nest;
using QueryableAsset = Test_Search_Api.V1.Gateways.QueryableAsset;

namespace Test_Search_Api.Tests.V1.E2ETests.Fixtures
{
    public class AssetFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "assets";
        public static AssetStub[] Assets =
        {
            new AssetStub{Title = "Mr", FirstName = "John", LastName = "Doe", DOB = "01/01/1980", AssetType = "House", NumberOfBedrooms = 2},
            new AssetStub{Title = "Ms", FirstName = "Jane", LastName = "Doe", DOB = "01/01/1970", AssetType = "Flat", NumberOfBedrooms = 3},
            new AssetStub{Title = "Mx", FirstName = "Jerry", LastName = "Seinfeld", DOB = "01/01/1960", AssetType = "Maisonette", NumberOfBedrooms = 1}
        };


        public AssetFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            WaitForESInstance();
        }

        public void GivenAnAssetIndexExists()
        {
            ElasticSearchClient.Indices.Delete(INDEX);

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(INDEX)).Exists)
            {
                var assetSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/assetIndex.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, assetSettingsDoc)
                    .ConfigureAwait(true);

                var assets = CreateAssetData();
                var awaitable = ElasticSearchClient.IndexManyAsync(assets, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }

        private List<QueryableAsset> CreateAssetData()
        {
            var listOfAssets = new List<QueryableAsset>();
            var fixture = new Fixture();
            var asset = fixture.Create<QueryableAsset>();
               
            listOfAssets.Add(asset);

            return listOfAssets;
        }

        public class AssetStub
        {
            public string Title { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DOB { get; set; }
            public string AssetType { get; set; }
            public int NumberOfBedrooms { get; set; }
        }
    }
}

