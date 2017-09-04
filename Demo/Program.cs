using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Starcounter;
using static Starcounter.Linq.DbLinq;

namespace Demo
{
    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                var roger = new Employee()
                {
                    Gender = Gender.Male,
                    Name = "Roger",
                    Age = 41,
                    Department = new Department()
                    {
                        Name = "Solution Architecture",
                        Company = new Company()
                        {
                            Name = "Starcounter"
                        }
                    }
                };


                var persons = Objects<Person>().Where(p => p.Name == "Roger").ToList();
                var persons1 = Objects<Person>().Take(1).ToList();
                var persons2 = Objects<Person>().Skip(1).ToList();
                var person = Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
                var person2 = Objects<Person>().FirstOrDefault(p => p.Name != "Roger");
                //Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "Starcounter");
                //Objects<Person>().FirstOrDefault(p => p.Name.Contains("oge"));
                //Objects<Person>().FirstOrDefault(p => !p.Name.Contains("oge"));
                //Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("Ro"));
                //Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("er"));
                //Objects<Person>().FirstOrDefault(p => p.Age > 0 && p.Age < 100);
                //Objects<Person>().FirstOrDefault(p => p.Name == null);
                //Objects<Person>().FirstOrDefault(p => p.Gender == Gender.Male);
                //Objects<Employee>().FirstOrDefault(p => p.Department == roger.Department);

                //Objects<Person>().Sum(p => p.Age);
                //Objects<Person>().Min(p => p.Age);
                //Objects<Person>().Max(p => p.Age);

                ////Objects<Person>().Ave rage(p => p.Age);

                //Objects<Person>().Count();

                //var ages = new[] {1, 2, 3, 4, 5};
                //Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));

                var cnt = Objects<Person>().Count();
                var avg = Objects<Person>().Average(x => x.Age);
                var min = Objects<Person>().Min(x => x.Age);
                var max = Objects<Person>().Max(x => x.Age);
                var sum = Objects<Person>().Sum(x => x.Age);
                var cnt2 = Objects<Person>().Count(x => x is Employee);

                Handle.GET("/sql", () =>
                {
                    var sw = Stopwatch.StartNew();
                    for (int i = 0; i < 1000000; i++)
                    {
                        var res = Db.SQL<Person>("SELECT p from Demo.Person p WHERE p.Name = ?", "Roger").First;
                    }
                    sw.Stop();
                    return sw.Elapsed.ToString();
                });


                Handle.GET("/compiled", () =>
                {

                    var sw = Stopwatch.StartNew();
                    for (int i = 0; i < 1000000; i++)
                    {
                        var res = Person.FirstNamed("Roger");
                    }
                    sw.Stop();
                    return sw.Elapsed.ToString();
                });

                Handle.GET("/linq", () =>
                {

                    var sw = Stopwatch.StartNew();
                    for (int i = 0; i < 1000000; i++)
                    {
                        //this just traverses the linq expression tree, it doesnt touch the DB
                        //var res = Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
                        var taken10 = Objects<Person>().Take(10).ToList();
                    }
                    sw.Stop();
                    return sw.Elapsed.ToString();
                });
            });
        }
    }



    [Database]
    public class Company
    {
        private static readonly Func<Company, IEnumerable<Department>> DepartmentsByCompany =
            CompileQuery((Company com) =>
                Objects<Department>().Where(e => e.Company == com));

        public string Name { get; set; }

        public IEnumerable<Department> Departments => DepartmentsByCompany(this);
    }

    [Database]
    public class Department
    {
        private static readonly Func<Department, IEnumerable<Employee>> EmployeesByDepartment =
            CompileQuery((Department dep) =>
                Objects<Employee>().Where(e => e.Department == dep));

        public Company Company { get; set; }
        public string Name { get; set; }

        public IEnumerable<Employee> Employees => EmployeesByDepartment(this);
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
        public static readonly Func<string, Person> FirstNamed =
            CompileQuery((string name) =>
                Objects<Person>().FirstOrDefault(p => p.Name == name));

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
    }
}