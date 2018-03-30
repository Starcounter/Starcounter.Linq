using System.Collections;
using Xunit;
using Starcounter.Linq;
using Starcounter.Nova;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    public class QueryableTests : IClassFixture<TestAppHost>
    {
        private readonly TestAppHost _fixture;

        public QueryableTests(TestAppHost fixture)
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
    }
}