using System;
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


                Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
                Objects<Person>().FirstOrDefault(p => p.Name != "Roger");
                Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "Starcounter");
                Objects<Person>().FirstOrDefault(p => p.Name.Contains("oge"));
                Objects<Person>().FirstOrDefault(p => !p.Name.Contains("oge"));
                Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("Ro"));
                Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("er"));
                Objects<Person>().FirstOrDefault(p => p.Age > 0 && p.Age < 100);
                Objects<Person>().FirstOrDefault(p => p.Name == null);
                Objects<Person>().FirstOrDefault(p => p.Gender == Gender.Male);

                Objects<Person>().Sum(p => p.Age);
                Objects<Person>().Min(p => p.Age);
                Objects<Person>().Max(p => p.Age);

                //X6Decimal mumbo jumbo
                //Objects<Person>().Average(p => p.Age);

                Objects<Person>().Count();

                var ages = new[] {1, 2, 3, 4, 5};
                Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
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