using System.Collections;
using Xunit;
using Starcounter;
using Starcounter.Linq;

namespace StarcounterLinqUnitTests.Tests
{
    public class QueryableTests
    {
        [Fact]
        public void TestGetEnumerator_IEnumerable()
        {
            Scheduling.RunTask(() =>
            {
                var enumerable = (IEnumerable)DbLinq.Objects<Person>();
                var enumerator = enumerable.GetEnumerator();
                Assert.NotNull(enumerator);
            }).Wait();
        }
    }
}