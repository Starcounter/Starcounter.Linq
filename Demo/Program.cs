using System;
using System.Linq;
using Starcounter;
using static PoS.Infra.DbLinq;

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

                Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
                Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "Starcounter");
                Objects<Person>().FirstOrDefault(p => p.Name.Contains("oge"));
                Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("Ro"));
                Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("er"));
                Objects<Person>().FirstOrDefault(p => p.Age > 0 && p.Age < 100);
                Objects<Person>().FirstOrDefault(p => p.Name == null);
                Objects<Person>().Sum(p => p.Age);
            });
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

    [Database]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}