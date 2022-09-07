using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Nest;
using Test_Search_Api.V1.Gateways;
using QueryableAsset = Test_Search_Api.V1.Gateways.QueryableAsset;

namespace Test_Search_Api.Tests.V1.E2ETests.Fixtures
{
    public class AssetFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "assets";

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
            var random = new Random();

            foreach (var value in Addresses)
            {
                var asset = fixture.Create<QueryableAsset>();
                asset.AssetAddress.AddressLine1 = value.FirstLine;
                asset.AssetType = value.AssetType;
                asset.AssetAddress.PostCode = value.PostCode;
                asset.AssetAddress.Uprn = value.UPRN;
                asset.AssetStatus = value.AssetStatus;
                asset.AssetCharacteristics.NumberOfBedSpaces = value.NoOfBedSpaces;
                asset.AssetCharacteristics.NumberOfCots = value.NoOfCots;
                asset.AssetCharacteristics.HasStairs = value.HasStairs;
                asset.AssetCharacteristics.HasPrivateBathroom = value.PrivateBathroom;
                asset.AssetCharacteristics.HasPrivateKitchen = value.PrivateKitchen;
                asset.AssetCharacteristics.IsStepFree = value.StepFree;
                listOfAssets.Add(asset);
            }

            return listOfAssets;
        }
    }
}

