using System;
using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class AggregationTests
    {
        private readonly DataTestFixture _fixture;

        public AggregationTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageInt32(Mode mode)
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
        public void AverageInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitInt8))()
                    : Objects<Person>().Average(x => x.LimitInt8);

                Assert.Equal(7D / 2, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitInt16))()
                    : Objects<Person>().Average(x => x.LimitInt16);

                Assert.Equal(7D / 2, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitInt64))()
                    : Objects<Person>().Average(x => x.LimitInt64);

                Assert.Equal(7D / 2, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitUInt8))()
                    : Objects<Person>().Average(x => x.LimitUInt8);

                Assert.Equal(7D / 2, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitUInt16))()
                    : Objects<Person>().Average(x => x.LimitUInt16);

                Assert.Equal(7D / 2, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitUInt32))()
                    : Objects<Person>().Average(x => x.LimitUInt32);

                Assert.Equal(7D / 2, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitDecimal))()
                    : Objects<Person>().Average(x => x.LimitDecimal);

                Assert.Equal(7.82M / 2, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitSingle))()
                    : Objects<Person>().Average(x => x.LimitSingle);

                Assert.Equal(3.9F, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.LimitDouble))()
                    : Objects<Person>().Average(x => x.LimitDouble);

                Assert.True(Math.Abs(3.89D - avg) < 1e-6);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinInt32(Mode mode)
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
        public void MinInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitInt8))()
                    : Objects<Person>().Min(x => x.LimitInt8);
                Assert.Equal(3, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitInt16))()
                    : Objects<Person>().Min(x => x.LimitInt16);
                Assert.Equal(3, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitInt64))()
                    : Objects<Person>().Min(x => x.LimitInt64);
                Assert.Equal(3, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitUInt32))()
                    : Objects<Person>().Min(x => x.LimitUInt32);
                Assert.Equal(3U, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitUInt8))()
                    : Objects<Person>().Min(x => x.LimitUInt8);
                Assert.Equal(3, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitUInt16))()
                    : Objects<Person>().Min(x => x.LimitUInt16);
                Assert.Equal(3, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinUInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitUInt64))()
                    : Objects<Person>().Min(x => x.LimitUInt64);
                Assert.Equal(3UL, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitSingle))()
                    : Objects<Person>().Min(x => x.LimitSingle);
                Assert.Equal(3.34F, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitDouble))()
                    : Objects<Person>().Min(x => x.LimitDouble);
                Assert.Equal(3.33D, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.LimitDecimal))()
                    : Objects<Person>().Min(x => x.LimitDecimal);
                Assert.Equal(3.35M, min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinString(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Min(x => x.Name))()
                    : Objects<Person>().Min(x => x.Name);
                Assert.Equal("Anton", min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinBoolean(Mode mode)
        {
            Db.Transact(() =>
            {
                var min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Department>().Min(x => x.Global))()
                    : Objects<Department>().Min(x => x.Global);
                Assert.False(min);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxInt32(Mode mode)
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
        public void MaxInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitInt8))()
                    : Objects<Person>().Max(x => x.LimitInt8);
                Assert.Equal(4, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitInt16))()
                    : Objects<Person>().Max(x => x.LimitInt16);
                Assert.Equal(4, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitInt64))()
                    : Objects<Person>().Max(x => x.LimitInt64);
                Assert.Equal(4, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitUInt32))()
                    : Objects<Person>().Max(x => x.LimitUInt32);
                Assert.Equal(4U, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitUInt8))()
                    : Objects<Person>().Max(x => x.LimitUInt8);
                Assert.Equal(4, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitUInt16))()
                    : Objects<Person>().Max(x => x.LimitUInt16);
                Assert.Equal(4, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxUInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                var Max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitUInt64))()
                    : Objects<Person>().Max(x => x.LimitUInt64);
                Assert.Equal(4UL, Max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitSingle))()
                    : Objects<Person>().Max(x => x.LimitSingle);
                Assert.Equal(4.46F, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitDouble))()
                    : Objects<Person>().Max(x => x.LimitDouble);
                Assert.Equal(4.45D, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.LimitDecimal))()
                    : Objects<Person>().Max(x => x.LimitDecimal);
                Assert.Equal(4.47M, max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxString(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Max(x => x.Name))()
                    : Objects<Person>().Max(x => x.Name);
                Assert.Equal("Roger", max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxBoolean(Mode mode)
        {
            Db.Transact(() =>
            {
                var max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Department>().Max(x => x.Global))()
                    : Objects<Department>().Max(x => x.Global);
                Assert.True(max);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumInt32(Mode mode)
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
        public void SumInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitInt8))()
                    : Objects<Person>().Sum(x => x.LimitInt8);

                Assert.Equal(7D, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitInt16))()
                    : Objects<Person>().Sum(x => x.LimitInt16);

                Assert.Equal(7D, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitInt64))()
                    : Objects<Person>().Sum(x => x.LimitInt64);

                Assert.Equal(7D, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitUInt8))()
                    : Objects<Person>().Sum(x => x.LimitUInt8);

                Assert.Equal(7D, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitUInt16))()
                    : Objects<Person>().Sum(x => x.LimitUInt16);

                Assert.Equal(7D, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitUInt32))()
                    : Objects<Person>().Sum(x => x.LimitUInt32);

                Assert.Equal(7D, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitDecimal))()
                    : Objects<Person>().Sum(x => x.LimitDecimal);

                Assert.Equal(7.82M, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitSingle))()
                    : Objects<Person>().Sum(x => x.LimitSingle);

                Assert.Equal(7.8F, sum);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                var sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Sum(x => x.LimitDouble))()
                    : Objects<Person>().Sum(x => x.LimitDouble);

                Assert.Equal(7.78, sum);
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
        public void LongCount(Mode mode)
        {
            Db.Transact(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().LongCount())()
                    : Objects<Person>().LongCount();

                Assert.Equal(2, cnt);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void LongCountWithPredicate(Mode mode)
        {
            Db.Transact(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().LongCount(x => x is Employee))()
                    : Objects<Person>().LongCount(x => x is Employee);

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
        public void OrderBy_LongCount(Mode mode)
        {
            Db.Transact(() =>
            {
                var cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderBy(x => x.Age).LongCount())()
                    : Objects<Person>().OrderBy(x => x.Age).LongCount();

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

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_Count(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Age).Select(x => x.Count()))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Age).Select(x => x.Count()).ToArray();

                Assert.Equal(2, cnt.Length);
                Assert.Equal(1, cnt[0]);
                Assert.Equal(1, cnt[1]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_CountPredicate(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery((Gender gender) => Objects<Person>().GroupBy(x => x.Age).Select(x => x.Count(p => p.Gender == gender)))(Gender.Male).ToArray()
                    : Objects<Person>().GroupBy(x => x.Age).Select(x => x.Count(p => p.Gender == Gender.Male)).ToArray();

                Assert.Equal(2, cnt.Length);
                Assert.Equal(1, cnt[0]);
                Assert.Equal(1, cnt[1]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_LongCount(Mode mode)
        {
            Db.Transact(() =>
            {
                long[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Age).Select(x => x.LongCount()))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Age).Select(x => x.LongCount()).ToArray();

                Assert.Equal(2, cnt.Length);
                Assert.Equal(1, cnt[0]);
                Assert.Equal(1, cnt[1]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_LongCountPredicate(Mode mode)
        {
            Db.Transact(() =>
            {
                long[] cnt = mode == Mode.CompiledQuery
                    ? CompileQuery((Gender gender) => Objects<Person>().GroupBy(x => x.Age).Select(x => x.LongCount(p => p.Gender == gender)))(Gender.Male).ToArray()
                    : Objects<Person>().GroupBy(x => x.Age).Select(x => x.LongCount(p => p.Gender == Gender.Male)).ToArray();

                Assert.Equal(2, cnt.Length);
                Assert.Equal(1, cnt[0]);
                Assert.Equal(1, cnt[1]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.Age)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.Age)).ToArray();

                Assert.Single(max);
                Assert.Equal(41, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                sbyte[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitInt8)).ToArray();

                Assert.Single(max);
                Assert.Equal(4, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                short[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitInt16)).ToArray();

                Assert.Single(max);
                Assert.Equal(4, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                long[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitInt64)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitInt64)).ToArray();

                Assert.Single(max);
                Assert.Equal(4, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                uint[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt32)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt32)).ToArray();

                Assert.Single(max);
                Assert.Equal(4U, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                byte[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt8)).ToArray();

                Assert.Single(max);
                Assert.Equal(4, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                ushort[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt16)).ToArray();

                Assert.Single(max);
                Assert.Equal(4, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxUInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                ulong[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt64)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitUInt64)).ToArray();

                Assert.Single(max);
                Assert.Equal(4UL, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                float[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitSingle)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitSingle)).ToArray();

                Assert.Single(max);
                Assert.Equal(4.46F, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitDouble)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitDouble)).ToArray();

                Assert.Single(max);
                Assert.Equal(4.45D, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                decimal[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitDecimal)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.LimitDecimal)).ToArray();

                Assert.Single(max);
                Assert.Equal(4.47M, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxBool(Mode mode)
        {
            Db.Transact(() =>
            {
                bool[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Department>().GroupBy(x => x.Company).Select(x => x.Max(p => p.Global)))().ToArray()
                    : Objects<Department>().GroupBy(x => x.Company).Select(x => x.Max(p => p.Global)).ToArray();

                Assert.Single(max);
                Assert.Equal(true, max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MaxString(Mode mode)
        {
            Db.Transact(() =>
            {
                string[] max = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.Name)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Max(p => p.Name)).ToArray();

                Assert.Single(max);
                Assert.Equal("Roger", max[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.Age)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.Age)).ToArray();

                Assert.Single(min);
                Assert.Equal(31, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                sbyte[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitInt8)).ToArray();

                Assert.Single(min);
                Assert.Equal(3, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                short[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitInt16)).ToArray();

                Assert.Single(min);
                Assert.Equal(3, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                long[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitInt64)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitInt64)).ToArray();

                Assert.Single(min);
                Assert.Equal(3, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                uint[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt32)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt32)).ToArray();

                Assert.Single(min);
                Assert.Equal(3U, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                byte[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt8)).ToArray();

                Assert.Single(min);
                Assert.Equal(3, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                ushort[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt16)).ToArray();

                Assert.Single(min);
                Assert.Equal(3, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinUInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                ulong[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt64)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitUInt64)).ToArray();

                Assert.Single(min);
                Assert.Equal(3UL, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                float[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitSingle)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitSingle)).ToArray();

                Assert.Single(min);
                Assert.Equal(3.34F, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitDouble)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitDouble)).ToArray();

                Assert.Single(min);
                Assert.Equal(3.33D, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                decimal[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitDecimal)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.LimitDecimal)).ToArray();

                Assert.Single(min);
                Assert.Equal(3.35M, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinBool(Mode mode)
        {
            Db.Transact(() =>
            {
                bool[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Department>().GroupBy(x => x.Company).Select(x => x.Min(p => p.Global)))().ToArray()
                    : Objects<Department>().GroupBy(x => x.Company).Select(x => x.Min(p => p.Global)).ToArray();

                Assert.Single(min);
                Assert.Equal(false, min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_MinString(Mode mode)
        {
            Db.Transact(() =>
            {
                string[] min = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.Name)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Min(p => p.Name)).ToArray();

                Assert.Single(min);
                Assert.Equal("Anton", min[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt32)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt32)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt8)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt16)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                long[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt64)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitInt64)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                long[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitUInt32)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitUInt32)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitUInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitUInt8)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                int[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitUInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitUInt16)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                float[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitSingle)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitSingle)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7.8F, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitDouble)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitDouble)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7.78, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_SumDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                decimal[] sum = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitDecimal)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Sum(p => p.LimitDecimal)).ToArray();

                Assert.Single(sum);
                Assert.Equal(7.82M, sum[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt32)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt32)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7D / 2, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt8)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7D / 2, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt16)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7D / 2, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgInt64(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt64)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitInt64)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7D / 2, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgUInt32(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitUInt32)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitUInt32)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7D / 2, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgUInt8(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitUInt8)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitUInt8)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7D / 2, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgUInt16(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitUInt16)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitUInt16)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7D / 2, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgSingle(Mode mode)
        {
            Db.Transact(() =>
            {
                float[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitSingle)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitSingle)).ToArray();

                Assert.Single(avg);
                Assert.Equal(3.9F, avg[0]);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgDouble(Mode mode)
        {
            Db.Transact(() =>
            {
                double[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitDouble)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitDouble)).ToArray();

                Assert.Single(avg);
                Assert.True(Math.Abs(3.89D - avg[0]) < 1e-6);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void GroupBy_AvgDecimal(Mode mode)
        {
            Db.Transact(() =>
            {
                decimal[] avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitDecimal)))().ToArray()
                    : Objects<Person>().GroupBy(x => x.Gender).Select(x => x.Average(p => p.LimitDecimal)).ToArray();

                Assert.Single(avg);
                Assert.Equal(7.82M / 2, avg[0]);
            });
        }
    }
}
