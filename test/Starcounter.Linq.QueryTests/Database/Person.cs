using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter.Nova;
using static Starcounter.Linq.DbLinq;

// ReSharper disable once CheckNamespace
namespace Starcounter.Linq.QueryTests
{
    [Database]
    public abstract class Person : INamed
    {
        public static readonly Func<string, Person> FirstNamed =
            CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name == name));

        //linq linq support
        public static readonly Func<int, int, IEnumerable<Person>> WithinAgeRange =
            CompileQuery((int min, int max) =>
                from p in Objects<Person>()
                where p.Age <= max
                where p.Age >= min
                select p
            );

        public abstract Gender Gender { get; set; }
        public abstract string Name { get; set; }
        public abstract int Age { get; set; }
        public abstract sbyte LimitInt8 { get; set; }
        public abstract short LimitInt16 { get; set; }
        public abstract int LimitInt32 { get; set; }
        public abstract long LimitInt64 { get; set; }
        public abstract byte LimitUInt8 { get; set; }
        public abstract ushort LimitUInt16 { get; set; }
        public abstract uint LimitUInt32 { get; set; }
        public abstract ulong LimitUInt64 { get; set; }
        public abstract decimal LimitDecimal { get; set; }
        public abstract float LimitSingle { get; set; }
        public abstract double LimitDouble { get; set; }
        public abstract Office Office { get; set; }
    }
}
