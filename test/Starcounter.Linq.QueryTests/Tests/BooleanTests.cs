using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class BooleanTests
    {
        private readonly DataTestFixture _fixture;

        public BooleanTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanTrue(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().Where(x => x.Disabled))().ToList()
                    : Objects<Employee>().Where(x => x.Disabled).ToList();
                Assert.Single(employees);
                Assert.Equal("Roger", employees.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanFalse(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().Where(x => !x.Disabled))().ToList()
                    : Objects<Employee>().Where(x => !x.Disabled).ToList();
                Assert.Single(employees);
                Assert.Equal("Anton", employees.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanFalse_Nested(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().Where(x => !x.Department.Global))().ToList()
                    : Objects<Employee>().Where(x => !x.Department.Global).ToList();
                Assert.Empty(employees);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanTrue_NestedTwice(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().Where(x => x.Department.Company.Global))().ToList()
                    : Objects<Employee>().Where(x => x.Department.Company.Global).ToList();
                Assert.Equal(2, employees.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEquals(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Disabled == fl))(true).ToList()
                    // ReSharper disable once RedundantBoolCompare
                    : Objects<Employee>().Where(x => x.Disabled == true).ToList();
                Assert.Single(employees);
                Assert.Equal("Roger", employees.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsNot(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Disabled != fl))(true).ToList()
                    : Objects<Employee>().Where(x => x.Disabled != true).ToList();
                Assert.Single(employees);
                Assert.Equal("Anton", employees.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEquals_Variable_Nested(Mode mode)
        {
            Db.Transact(() =>
            {
                var global = true;
                var employees = mode == Mode.CompiledQuery
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Global == fl))(global).ToList()
                    : Objects<Employee>().Where(x => x.Department.Global == global).ToList();
                Assert.Equal(2, employees.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsNot_Variable_Nested(Mode mode)
        {
            Db.Transact(() =>
            {
                var global = false;
                var employees = mode == Mode.CompiledQuery
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Global != fl))(global).ToList()
                    : Objects<Employee>().Where(x => x.Department.Global != global).ToList();
                Assert.Equal(2, employees.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEquals_NestedTwice(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Company.Global == fl))(true).ToList()
                    // ReSharper disable once RedundantBoolCompare
                    : Objects<Employee>().Where(x => x.Department.Company.Global == true).ToList();
                Assert.Equal(2, employees.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsNot_NestedTwice(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Company.Global != fl))(true).ToList()
                    : Objects<Employee>().Where(x => x.Department.Company.Global != true).ToList();
                Assert.Empty(employees);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanTrueComplex(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Employee>().Where(x => x.Age > age && x.Disabled))(10).ToList()
                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled).ToList();
                Assert.Single(employees);
                Assert.Equal("Roger", employees.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanFalseComplex(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Employee>().Where(x => x.Age > age && !x.Disabled))(10).ToList()
                    : Objects<Employee>().Where(x => x.Age > 10 && !x.Disabled).ToList();
                Assert.Single(employees);
                Assert.Equal("Anton", employees.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsComplex(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age, bool fl) => Objects<Employee>().Where(x => x.Age > age && x.Disabled == fl))(10, true).ToList()
                    // ReSharper disable once RedundantBoolCompare
                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled == true).ToList();
                Assert.Single(employees);
                Assert.Equal("Roger", employees.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void BooleanEqualsNotComplex(Mode mode)
        {
            Db.Transact(() =>
            {
                var employees = mode == Mode.CompiledQuery
                    ? CompileQuery((int age, bool fl) => Objects<Employee>().Where(x => x.Age > age && x.Disabled != fl))(10, true).ToList()
                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled != true).ToList();
                Assert.Single(employees);
                Assert.Equal("Anton", employees.First().Name);
            });
        }
    }
}
