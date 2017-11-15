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

        [Fact]
        public void WhereStringEqual()
        {
            Scheduling.ScheduleTask(() =>
            {
                var persons = Objects<Person>().Where(p => p.Name == "Roger").ToList();
                Assert.Equal(1, persons.Count);
                Assert.Equal("Roger", persons.First().Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void WhereIs()
        {
            Scheduling.ScheduleTask(() =>
            {
                var persons = Objects<Person>().Where(p => p is Employee).ToList();
                Assert.Equal(2, persons.Count);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstStringEqual()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void WhereStringEqual_Take_FirstEnumEqual()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().Where(x => x.Name == "Roger").Take(10).FirstOrDefault(x => x.Gender == Gender.Male);
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
                Assert.Equal(Gender.Male, person.Gender);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstStringNotEqual()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => p.Name != "Roger");
                Assert.NotNull(person);
                Assert.NotEqual("Roger", person.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstNestedStringEqual()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "Starcounter");
                Assert.NotNull(person);
                Assert.Equal("Starcounter", person.Department.Company.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstStringContains()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => p.Name.Contains("oge"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstStringNotContains()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => !p.Name.Contains("oge"));
                Assert.NotNull(person);
                Assert.NotEqual("Roger", person.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstStringStartsWith()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("Ro"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstStringEndsWith()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("er"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstIntegerGreaterAndLess()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => p.Age > 20 && p.Age < 40);
                Assert.NotNull(person);
                Assert.Equal(31, person.Age);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstStringEqualsNull__NotFound()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().FirstOrDefault(p => p.Name == null);
                Assert.Null(person);
            }, waitForCompletion: true);
        }

        [Fact]
        public void First()
        {
            Scheduling.ScheduleTask(() =>
            {
                var person = Objects<Person>().First();
                Assert.NotNull(person);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstObjectEqual()
        {
            Scheduling.ScheduleTask(() =>
            {
                var employee1 = Objects<Employee>().First(p => p.Name == "Roger");
                var employee2 = Objects<Employee>().FirstOrDefault(p => p.Department == employee1.Department);
                Assert.NotNull(employee2);
                Assert.Equal("Roger", employee2.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstObjectNotEqual()
        {
            Scheduling.ScheduleTask(() =>
            {
                var employee1 = Objects<Employee>().First(p => p.Name == "Roger");
                var employee2 = Objects<Employee>().FirstOrDefault(p => p.Department != employee1.Department);
                Assert.NotNull(employee2);
                Assert.NotEqual("Roger", employee2.Name);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstIntegerInArray()
        {
            Scheduling.ScheduleTask(() =>
            {
                var ages = new[] { 41, 42, 43 };
                var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
                Assert.NotNull(person);
                Assert.Equal(41, person.Age);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstIntegerInArray__NotFound()
        {
            Scheduling.ScheduleTask(() =>
            {
                var ages = new[] { 1, 2, 3, 4, 5 };
                var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
                Assert.Null(person);
            }, waitForCompletion: true);
        }

        [Fact]
        public void FirstIntegerNotInArray()
        {
            Scheduling.ScheduleTask(() =>
            {
                var ages = new[] { 1, 2, 3, 4, 5 };
                var person = Objects<Person>().FirstOrDefault(p => !ages.Contains(p.Age));
                Assert.NotNull(person);
            }, waitForCompletion: true);
        }
    }
}