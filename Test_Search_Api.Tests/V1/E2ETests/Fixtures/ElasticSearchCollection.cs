using Test_Search_Api;
using Test_Search_Api.Tests;
using Xunit;

namespace Test_Search_Api.Tests.V1.E2ETests.Fixtures
{
    [CollectionDefinition("ElasticSearch collection", DisableParallelization = true)]
    public class ElasticSearchCollection : ICollectionFixture<MockWebApplicationFactory<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

