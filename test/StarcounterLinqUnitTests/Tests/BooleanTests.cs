//using System.Linq;
//using Xunit;
//using static Starcounter.Linq.DbLinq;

//namespace StarcounterLinqUnitTests.Tests
//{
//    public class BooleanTests : IClassFixture<BaseTestsFixture>
//    {
//        public BooleanTests(BaseTestsFixture fixture)
//        {
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanTrue(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery(() => Objects<Employee>().Where(x => x.Disabled))().ToList()
//                    : Objects<Employee>().Where(x => x.Disabled).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Roger", employees.First().Name);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanFalse(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery(() => Objects<Employee>().Where(x => !x.Disabled))().ToList()
//                    : Objects<Employee>().Where(x => !x.Disabled).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Anton", employees.First().Name);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanFalse_Nested(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery(() => Objects<Employee>().Where(x => !x.Department.Global))().ToList()
//                    : Objects<Employee>().Where(x => !x.Department.Global).ToList();
//                Assert.Equal(0, employees.Count);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanTrue_NestedTwice(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery(() => Objects<Employee>().Where(x => x.Department.Company.Global))().ToList()
//                    : Objects<Employee>().Where(x => x.Department.Company.Global).ToList();
//                Assert.Equal(2, employees.Count);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEquals(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Disabled == fl))(true).ToList()
//                    : Objects<Employee>().Where(x => x.Disabled == true).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Roger", employees.First().Name);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEqualsNot(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Disabled != fl))(true).ToList()
//                    : Objects<Employee>().Where(x => x.Disabled != true).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Anton", employees.First().Name);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEquals_Variable_Nested(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var global = true;
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Global == fl))(global).ToList()
//                    : Objects<Employee>().Where(x => x.Department.Global == global).ToList();
//                Assert.Equal(2, employees.Count);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEqualsNot_Variable_Nested(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var global = false;
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Global != fl))(global).ToList()
//                    : Objects<Employee>().Where(x => x.Department.Global != global).ToList();
//                Assert.Equal(2, employees.Count);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEquals_NestedTwice(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Company.Global == fl))(true).ToList()
//                    : Objects<Employee>().Where(x => x.Department.Company.Global == true).ToList();
//                Assert.Equal(2, employees.Count);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEqualsNot_NestedTwice(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((bool fl) => Objects<Employee>().Where(x => x.Department.Company.Global != fl))(true).ToList()
//                    : Objects<Employee>().Where(x => x.Department.Company.Global != true).ToList();
//                Assert.Equal(0, employees.Count);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanTrueComplex(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((int age) => Objects<Employee>().Where(x => x.Age > age && x.Disabled))(10).ToList()
//                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Roger", employees.First().Name);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanFalseComplex(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((int age) => Objects<Employee>().Where(x => x.Age > age && !x.Disabled))(10).ToList()
//                    : Objects<Employee>().Where(x => x.Age > 10 && !x.Disabled).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Anton", employees.First().Name);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEqualsComplex(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((int age, bool fl) => Objects<Employee>().Where(x => x.Age > age && x.Disabled == fl))(10, true).ToList()
//                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled == true).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Roger", employees.First().Name);
//            }).Wait();
//        }

//        [Theory]
//        [InlineData(Mode.AdHoc)]
//        [InlineData(Mode.CompiledQuery)]
//        public void BooleanEqualsNotComplex(Mode mode)
//        {
//            Scheduling.RunTask(() =>
//            {
//                var employees = mode == Mode.CompiledQuery
//                    ? CompileQuery((int age, bool fl) => Objects<Employee>().Where(x => x.Age > age && x.Disabled != fl))(10, true).ToList()
//                    : Objects<Employee>().Where(x => x.Age > 10 && x.Disabled != true).ToList();
//                Assert.Equal(1, employees.Count);
//                Assert.Equal("Anton", employees.First().Name);
//            }).Wait();
//        }
//    }
//}
