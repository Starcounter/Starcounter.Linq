using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starcounter;
using static Starcounter.Linq.DbLinq;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                var res = EmptyObjects<Person>().FirstOrDefault(p => p.Name == "Roger");
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
        Female,
    }

    [Database]
    public class Person
    {
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
