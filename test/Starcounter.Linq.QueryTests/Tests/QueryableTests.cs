using System.Collections;
using Xunit;
using Starcounter.Nova;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    public class QueryableTests : IClassFixture<BaseTestsFixture>
    {
        private readonly BaseTestsFixture _fixture;

        public QueryableTests(BaseTestsFixture fixture)
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