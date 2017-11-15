using System;

namespace Starcounter.Linq.Tests
{
    public class Person
    {
        public static readonly Func<string, Person> FirstNamed = DbLinq.CompileQuery((string name) => DbLinq.Objects<Person>().FirstOrDefault(p => p.Name == name));

        public Gender Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Limit { get; set; }
    }
}