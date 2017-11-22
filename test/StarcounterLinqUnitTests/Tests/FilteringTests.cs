using System.Linq;
using Starcounter;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace StarcounterLinqUnitTests.Tests
{
    public class FilteringTests : IClassFixture<BaseTestsFixture>
    {
        public FilteringTests(BaseTestsFixture fixture)
        {
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereStringEqual(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Person>().Where(p => p.Name == name))("Roger").ToList()
                    : Objects<Person>().Where(p => p.Name == "Roger").ToList();
                Assert.Equal(1, persons.Count);
                Assert.Equal("Roger", persons.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereIntEqual_ReservedWordField(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery((int limit) => Objects<Person>().Where(p => p.Limit == limit))(2).ToList()
                    : Objects<Person>().Where(p => p.Limit == 2).ToList();
                Assert.Equal(1, persons.Count);
                Assert.Equal("Roger", persons.First().Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereIs(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Where(p => p is Employee))().ToList()
                    : Objects<Person>().Where(p => p is Employee).ToList();
                Assert.Equal(2, persons.Count);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstStringEqual(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name == name))("Roger")
                    : Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereStringEqual_Take_FirstEnumEqual(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name, Gender gender) => Objects<Person>().Where(x => x.Name == name).Take(10).FirstOrDefault(x => x.Gender == gender))("Roger", Gender.Male)
                    : Objects<Person>().Where(x => x.Name == "Roger").Take(10).FirstOrDefault(x => x.Gender == Gender.Male);
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
                Assert.Equal(Gender.Male, person.Gender);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstStringNotEqual(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name != name))("Roger")
                    : Objects<Person>().FirstOrDefault(p => p.Name != "Roger");
                Assert.NotNull(person);
                Assert.NotEqual("Roger", person.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstNestedStringEqual(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == name))("Starcounter")
                    : Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "Starcounter");
                Assert.NotNull(person);
                Assert.Equal("Starcounter", person.Department.Company.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstStringContains(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string namePart) => Objects<Person>().FirstOrDefault(p => p.Name.Contains(namePart)))("oge")
                    : Objects<Person>().FirstOrDefault(p => p.Name.Contains("oge"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstStringNotContains(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string namePart) => Objects<Person>().FirstOrDefault(p => !p.Name.Contains(namePart)))("oge")
                    : Objects<Person>().FirstOrDefault(p => !p.Name.Contains("oge"));
                Assert.NotNull(person);
                Assert.NotEqual("Roger", person.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstStringStartsWith(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string nameStart) => Objects<Person>().FirstOrDefault(p => p.Name.StartsWith(nameStart)))("Ro")
                    : Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("Ro"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstStringEndsWith(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string nameEnd) => Objects<Person>().FirstOrDefault(p => p.Name.EndsWith(nameEnd)))("er")
                    : Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("er"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstIntegerGreaterAndLess(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((int min, int max) => Objects<Person>().FirstOrDefault(p => p.Age > min && p.Age < max))(20, 40)
                    : Objects<Person>().FirstOrDefault(p => p.Age > 20 && p.Age < 40);
                Assert.NotNull(person);
                Assert.Equal(31, person.Age);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstStringEqualsNull__NotFound(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name == name))(null)
                    : Objects<Person>().FirstOrDefault(p => p.Name == null);
                Assert.Null(person);
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
        public void FirstObjectEqual(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employee1 = Objects<Employee>().First(p => p.Name == "Roger");
                var employee2 = mode == Mode.CompiledQuery
                    ? CompileQuery((Department department) => Objects<Employee>().FirstOrDefault(p => p.Department == department))(employee1.Department)
                    : Objects<Employee>().FirstOrDefault(p => p.Department == employee1.Department);
                Assert.NotNull(employee2);
                Assert.Equal("Roger", employee2.Name);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstObjectNotEqual(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var employee1 = Objects<Employee>().First(p => p.Name == "Roger");
                var employee2 = mode == Mode.CompiledQuery
                    ? CompileQuery((Department department) => Objects<Employee>().FirstOrDefault(p => p.Department != department))(employee1.Department)
                    : Objects<Employee>().FirstOrDefault(p => p.Department != employee1.Department);
                Assert.NotNull(employee2);
                Assert.NotEqual("Roger", employee2.Name);
            }).Wait();
        }

        [Fact]
        public void FirstIntegerInArray__AdHoc()
        {
            Scheduling.RunTask(() =>
            {
                var ages = new[] { 41, 42, 43 };
                var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
                Assert.NotNull(person);
                Assert.Equal(41, person.Age);
            }).Wait();
        }

        [Fact]
        public void FirstIntegerInArray__NotFound__AdHoc()
        {
            Scheduling.RunTask(() =>
            {
                var ages = new[] { 1, 2, 3, 4, 5 };
                var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
                Assert.Null(person);
            }).Wait();
        }

        [Fact]
        public void FirstIntegerNotInArray__AdHoc()
        {
            Scheduling.RunTask(() =>
            {
                var ages = new[] { 1, 2, 3, 4, 5 };
                var person = Objects<Person>().FirstOrDefault(p => !ages.Contains(p.Age));
                Assert.NotNull(person);
            }).Wait();
        }
    }
}