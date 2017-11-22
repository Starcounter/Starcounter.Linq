using System.Linq;
using Starcounter;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace StarcounterLinqUnitTests.Tests
{
    public class AggregationTests : IClassFixture<BaseTestsFixture>
    {
        public AggregationTests(BaseTestsFixture fixture)
        {
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageInteger(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.Age))()
                    : Objects<Person>().Average(x => x.Age);

                Assert.Equal(36, avg);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinInteger(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.Age))()
                    : Objects<Person>().Min(x => x.Age);
                Assert.Equal(31, min);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxInteger(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.Age))()
                    : Objects<Person>().Max(x => x.Age);
                Assert.Equal(41, max);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumInteger(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.Age))()
                    : Objects<Person>().Sum(x => x.Age);
                Assert.Equal(72, sum);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Count(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Count())()
                    : Objects<Person>().Count();
                Assert.Equal(2, cnt);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void CountWithPredicate(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Count(x => x is Employee))()
                    : Objects<Person>().Count(x => x is Employee);
                Assert.Equal(2, cnt);
            }).Wait();
        }
    }
}