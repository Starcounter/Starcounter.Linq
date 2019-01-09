using Xunit;

namespace Starcounter.Linq.QueryTests
{
    [CollectionDefinition("Data tests")]
    public class DataTestsCollection : ICollectionFixture<DataTestFixture>
    {
    }
}