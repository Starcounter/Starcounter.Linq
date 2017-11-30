using System.Linq;
using Starcounter;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace StarcounterLinqUnitTests.Tests
{
    public class BooleanTests : IClassFixture<BaseTestsFixture>
    {
        public BooleanTests(BaseTestsFixture fixture)
        {
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanTrue(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().Where(x => x.Disabled))().ToList()
                    : Objects<Employee>().Where(x => x.Disabled).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Roger", employees.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanFalse(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().Where(x => !x.Disabled))().ToList()
                    : Objects<Employee>().Where(x => !x.Disabled).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Anton", employees.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEquals(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Disabled == fl))(true).ToList()
                    : Objects<Employee>().Where(x => x.Disabled == true).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Roger", employees.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsNot(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Disabled != fl))(true).ToList()
                    : Objects<Employee>().Where(x => x.Disabled != true).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Anton", employees.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanTrueComplex(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Employee>().Where(x => x.Age > age && x.Disabled))(10).ToList()
                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Roger", employees.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanFalseComplex(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Employee>().Where(x => x.Age > age && !x.Disabled))(10).ToList()
                    : Objects<Employee>().Where(x => x.Age > 10 && !x.Disabled).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Anton", employees.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsComplex(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age, bool fl) => Objects<Employee>().Where(x => x.Age > age && x.Disabled == fl))(10, true).ToList()
                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled == true).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Roger", employees.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsNotComplex(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age, bool fl) => Objects<Employee>().Where(x => x.Age > age && x.Disabled != fl))(10, true).ToList()
                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled != true).ToList();
                Assert.Equal(1, employees.Count);
                Assert.Equal("Anton", employees.First().Name);
            }).Wait();
        }
    }
}
