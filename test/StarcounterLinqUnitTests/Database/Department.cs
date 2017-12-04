using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter;
using Starcounter.Linq;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public class Department
    {
        private static readonly Func<Department, IEnumerable<Employee>> EmployeesByDepartment =
            DbLinq.CompileQuery((Department dep) =>
                DbLinq.Objects<Employee>().Where(e => e.Department == dep));

        public Company Company { get; set; }
        public string Name { get; set; }

        public IEnumerable<Employee> Employees => EmployeesByDepartment(this);
        public bool Global { get; set; }
    }
}