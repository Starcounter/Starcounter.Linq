using System.Linq;
using Starcounter;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace StarcounterLinqUnitTests.Tests
{
    public class LinqTests : IClassFixture<BaseTestsFixture>
    {
        public LinqTests(BaseTestsFixture fixture)
        {
        }

        [Fact]
        public void Any_Predicate()
        {
            Scheduling.RunTask(() =>
            {
                var any = Objects<Person>().Any(x => x.Age > 0);
                Assert.True(any);
            }).Wait();
        }

        [Fact]
        public void Any()
        {
            Scheduling.RunTask(() =>
            {
                var any = Objects<Person>().Any();
                Assert.True(any);
            }).Wait();
        }
    }
}
