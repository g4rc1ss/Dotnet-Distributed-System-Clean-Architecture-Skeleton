using TestIntegracionAPI.Initializers;
using Xunit;

namespace TestIntegracionAPI
{
    [CollectionDefinition(TestCollections.WebApiTests)]
    public class TestApiConnectionFixture : ICollectionFixture<TestApiConnectionInitializer>
    {
    }


}
