using System.Linq;
using Starcounter;
using Starcounter.Linq;
using Xunit;

namespace StarcounterLinqUnitTests.Tests
{
    public class PagingTests : IClassFixture<BaseTestsFixture>
    {
        public PagingTests(BaseTestsFixture fixture)
        {
        }

        [Fact]
        public void Take()
        {
            Scheduling.ScheduleTask(() =>
            {
                var persons = DbLinq.Objects<Person>().Take(1).ToList();
                Assert.Equal(1, persons.Count);
            }, waitForCompletion: true);
        }

        [Fact]
        public void Skip()
        {
            Scheduling.ScheduleTask(() =>
            {
                var persons = DbLinq.Objects<Person>().Skip(1).ToList();
                Assert.Equal(1, persons.Count);
            }, waitForCompletion: true);
        }
    }
}