using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    public class AggregationTests : IClassFixture<BaseTestsFixture>
    {
        private readonly BaseTestsFixture _fixture;

        public AggregationTests(BaseTestsFixture fixture)
        {
            _fixture = fixture;
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

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy_Count(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderBy(x => x.Age).Count())()
                    : Objects<Person>().OrderBy(x => x.Age).Count();
                Assert.Equal(2, cnt);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy_SumInteger(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderBy(x => x.Age).Sum(x => x.Age))()
                    : Objects<Person>().OrderBy(x => x.Age).Sum(x => x.Age);
                Assert.Equal(72, sum);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_Count(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                int[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Age).Select(x => x.Count()))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Age).Select(x => x.Count()).ToArray();
                Assert.Equal(2, cnt.Length);
                Assert.Equal(1, cnt[0]);
                Assert.Equal(1, cnt[1]);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_Max(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                int[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.Age)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.Age)).ToArray();
                Assert.Equal(1, cnt.Length);
                Assert.Equal(41, cnt[0]);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_Min(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                int[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.Age)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.Age)).ToArray();
                Assert.Equal(1, cnt.Length);
                Assert.Equal(31, cnt[0]);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_Sum(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                int[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.Limit)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.Limit)).ToArray();
                Assert.Equal(1, cnt.Length);
                Assert.Equal(3, cnt[0]);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_Avg(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                double[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.Limit)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.Limit)).ToArray();
                Assert.Equal(1, cnt.Length);
                Assert.Equal(3D/2, cnt[0]);
            }).Wait();
        }
    }
}
