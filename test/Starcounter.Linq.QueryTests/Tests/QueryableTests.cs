using System.Collections;
using Xunit;
using Starcounter.Nova;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class QueryableTests
    {
        private readonly DataTestFixture _fixture;

        public QueryableTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestGetEnumerator_IEnumerable()
        {
            Db.Transact(() =>
            {
                var enumerable = (IEnumerable)Objects<Person>();
                var enumerator = enumerable.GetEnumerator();
                Assert.NotNull(enumerator);
            });
        }

        [Fact]
        public void TestGetEnumerable_ToString()
        {
            Db.Transact(() =>
            {
                var enumerable = Objects<Person>();
                Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"QueryTests\".\"Person\" P", enumerable.ToString());
            });
        }
    }
}