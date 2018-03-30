using System;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.SqlTests
{
    public class Person
    {
        public static readonly Func<string, Person> FirstNamed = CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name == name));

        public Gender Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Limit { get; set; }
    }
}