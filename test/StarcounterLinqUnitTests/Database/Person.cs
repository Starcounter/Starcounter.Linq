using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter;
using Starcounter.Linq;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public class Person
    {
        public static readonly Func<string, Person> FirstNamed =
            DbLinq.CompileQuery((string name) =>
                DbLinq.Objects<Person>().FirstOrDefault(p => p.Name == name));

        //linq linq support
        public static readonly Func<int, int, IEnumerable<Person>> WithinAgeRange =
            DbLinq.CompileQuery((int min, int max) =>
                from p in DbLinq.Objects<Person>()
                where p.Age <= max
                where p.Age >= min
                select p
            );

        public Gender Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}