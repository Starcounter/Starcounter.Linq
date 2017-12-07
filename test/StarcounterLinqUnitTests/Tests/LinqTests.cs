using System;
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

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void First(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().First())()
                    : Objects<Person>().First();
                Assert.NotNull(person);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void First_SequenceEmpty(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().First(x => x.Age == age))(100)
                        : Objects<Person>().First(x => x.Age == 100);
                });
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Where_First_SequenceEmpty(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().Where(x => x.Age == age).First())(100)
                        : Objects<Person>().Where(x => x.Age == 100).First();
                });
            }).Wait();
        }
    }
}
