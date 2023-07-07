using TestIntegracionHost.Initializers;
using Xunit;

namespace TestIntegracionHost
{
    [CollectionDefinition(TestCollections.WebApiTests)]
    public class TestApiConnectionFixture : ICollectionFixture<TestApiConnectionInitializer>
    {
    }


}
