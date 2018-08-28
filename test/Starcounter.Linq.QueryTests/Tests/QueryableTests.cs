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

        [Fact]
        public void TestGetEnumerable_ToString()
        {
            Scheduling.RunTask(() =>
            {
                var enumerable = Objects<Person>();
                Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"QueryTests\".\"Person\" P", enumerable.ToString());
            }).Wait();
        }
    }
}