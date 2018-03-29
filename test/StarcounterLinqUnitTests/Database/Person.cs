using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public abstract class Person
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

        public abstract Gender Gender { get; set; }
        public abstract string Name { get; set; }
        public abstract int Age { get; set; }
        public abstract int Limit { get; set; }
        public abstract Office Office { get; set; }
    }
}