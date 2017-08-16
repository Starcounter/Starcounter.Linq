using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name = ?))", 
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name == "XXX")));

            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE (((P.Name = ?) AND (P.Age > ?)))",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name == "XXX" && p.Age > 7)));
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
