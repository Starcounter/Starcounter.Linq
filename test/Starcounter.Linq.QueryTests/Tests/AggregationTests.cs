using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class AggregationTests : IClassFixture<DataTestFixture>
    {
        private readonly DataTestFixture _fixture;

        public AggregationTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageInteger(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.Age))()
                    : Objects<Person>().Average(x => x.Age);

                Assert.Equal(36, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinInteger(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.Age))()
                    : Objects<Person>().Min(x => x.Age);
                Assert.Equal(31, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxInteger(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.Age))()
                    : Objects<Person>().Max(x => x.Age);
                Assert.Equal(41, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumInteger(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.Age))()
                    : Objects<Person>().Sum(x => x.Age);
                Assert.Equal(72, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Count(Mode mode)
        {
            Db.Transact(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Count())()
                    : Objects<Person>().Count();
                Assert.Equal(2, cnt);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void CountWithPredicate(Mode mode)
        {
            Db.Transact(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Count(x => x is Employee))()
                    : Objects<Person>().Count(x => x is Employee);
                Assert.Equal(2, cnt);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy_Count(Mode mode)
        {
            Db.Transact(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderBy(x => x.Age).Count())()
                    : Objects<Person>().OrderBy(x => x.Age).Count();
                Assert.Equal(2, cnt);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy_SumInteger(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderBy(x => x.Age).Sum(x => x.Age))()
                    : Objects<Person>().OrderBy(x => x.Age).Sum(x => x.Age);
                Assert.Equal(72, sum);
            });
        }
    }
}