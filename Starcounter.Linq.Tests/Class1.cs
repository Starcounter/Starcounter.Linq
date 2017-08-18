using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.Tests
{
    public class LinqTests
    {
        private static string Sql<T>(Expression<Func<T>> exp) => new CompiledQuery<T>(exp).SqlStatement;

        [Fact]
        public void Run()
        {
            Assert.Equal("SELECT E FROM Starcounter.Linq.Tests.Employee E WHERE (((E.Department.Company.Name = ?) AND (E.Age > ?))) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "XXX" && p.Age > 123)));
        }

        [Fact]
        public void Take()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P FETCH 10",
                Sql(() => Objects<Person>().Take(10)));
        }

        [Fact]
        public void OrderBy()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P ORDER BY P.Age ASC",
                Sql(() => Objects<Person>().OrderBy(p => p.Age)));
        }

        [Fact]
        public void OrderByMultiple()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P ORDER BY P.Age ASC, P.Name DESC",
                Sql(() => Objects<Person>().OrderBy(p => p.Age).ThenByDescending(p => p.Name)));
        }

        [Fact]
        public void ValueEquals()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name = ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name == "XXX")));
        }

        [Fact]
        public void ValueGreaterThan()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Age > ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age > 123)));
        }

        [Fact]
        public void ValueGreaterOrEqualTo()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Age >= ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age >= 123)));
        }

        [Fact]
        public void StringContains()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name LIKE '%' || ? || '%')) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.Contains("XXX"))));
        }

        [Fact]
        public void StringStartsWith()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name LIKE ? || '%')) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("XXX"))));
        }

        [Fact]
        public void StringEndsWith()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name LIKE '%' || ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("XXX"))));
        }
    }
    public class Company
    {
        public string Name { get; set; }
    }

    public class Department
    {
        public Company Company { get; set; }
        public string Name { get; set; }
    }

    public class Employee : Person
    {
        public Department Department { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
    }

    public class Person
    {
        public static readonly Func<string, Person> FirstNamed = CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name == name));

        public Gender Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
