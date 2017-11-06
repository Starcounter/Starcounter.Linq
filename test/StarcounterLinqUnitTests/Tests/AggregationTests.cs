using System.Linq;
using Starcounter;
using Starcounter.Linq;
using Xunit;

namespace StarcounterLinqUnitTests.Tests
{
    public class AggregationTests : IClassFixture<BaseTestsFixture>
    {
        public AggregationTests(BaseTestsFixture fixture)
        {
        }

        [Fact]
        public void AverageInteger()
        {
            Scheduling.ScheduleTask(() =>
            {
                var avg = DbLinq.Objects<Person>().Average(x => x.Age);
                Assert.Equal(36, avg);
            }, waitForCompletion: true);
        }

        [Fact]
        public void MinInteger()
        {
            Scheduling.ScheduleTask(() =>
            {
                var min = DbLinq.Objects<Person>().Min(x => x.Age);
                Assert.Equal(31, min);
            }, waitForCompletion: true);
        }

        [Fact]
        public void MaxInteger()
        {
            Scheduling.ScheduleTask(() =>
            {
                var max = DbLinq.Objects<Person>().Max(x => x.Age);
                Assert.Equal(41, max);
            }, waitForCompletion: true);
        }

        [Fact]
        public void SumInteger()
        {
            Scheduling.ScheduleTask(() =>
            {
                var sum = DbLinq.Objects<Person>().Sum(x => x.Age);
                Assert.Equal(72, sum);
            }, waitForCompletion: true);
        }

        [Fact]
        public void Count()
        {
            Scheduling.ScheduleTask(() =>
            {
                var cnt = DbLinq.Objects<Person>().Count();
                Assert.Equal(2, cnt);
            }, waitForCompletion: true);
        }

        [Fact]
        public void CountWithPredicate()
        {
            Scheduling.ScheduleTask(() =>
            {
                var cnt = DbLinq.Objects<Person>().Count(x => x is Employee);
                Assert.Equal(2, cnt);
            }, waitForCompletion: true);
        }
    }
}