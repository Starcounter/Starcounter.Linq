using System.Collections;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    public class QueryableTests
    {
        [Fact]
        public void TestGetEnumerator_IEnumerable()
        {
            Scheduling.RunTask(() =>
            {
                var enumerable = (IEnumerable)Objects<Person>();
                var enumerator = enumerable.GetEnumerator();
                Assert.NotNull(enumerator);
            }).Wait();
        }
    }
}