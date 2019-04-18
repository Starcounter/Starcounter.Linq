using System;
using System.Collections.Generic;
using System.Linq;
using static Starcounter.Linq.DbLinq;

// ReSharper disable once CheckNamespace
namespace Starcounter.Linq.QueryTests
{
    [Database]
    public class Person : INamed
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

        public Gender Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public sbyte LimitInt8 { get; set; }
        public short LimitInt16 { get; set; }
        public int LimitInt32 { get; set; }
        public long LimitInt64 { get; set; }
        public byte LimitUInt8 { get; set; }
        public ushort LimitUInt16 { get; set; }
        public uint LimitUInt32 { get; set; }
        public ulong LimitUInt64 { get; set; }
        public decimal LimitDecimal { get; set; }
        public float LimitSingle { get; set; }
        public double LimitDouble { get; set; }
        public Office Office { get; set; }
    }
}