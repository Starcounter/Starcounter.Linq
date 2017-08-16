using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Starcounter;
using Starcounter.Linq;

namespace Sandbox
{
    internal class Program
    {
        public static Expression<Func<Person, bool>> ToExpression(Expression<Func<Person, bool>> predicate)
        {
            return predicate;
        }

        private static void Main(string[] args)
        {
            //var q = DbLinq.CompileQuery((string name) => DummyLinq.Objects<Person>().FirstOrDefault(p => p.Name == name));
            //q("Roger");

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < 3000000; i++)
            {
                var res = DummyLinq.Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }


    [Database]
    public class Company
    {
        public string Name { get; set; }
    }

    [Database]
    public class Department
    {
        public Company Company { get; set; }
        public string Name { get; set; }
    }

    [Database]
    public class Employee : Person
    {
        public Department Department { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    [Database]
    public class Person
    {
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}