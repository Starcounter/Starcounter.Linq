using System;
using System.Collections.Generic;
using System.Linq;
using static Starcounter.Linq.DbLinq;

// ReSharper disable once CheckNamespace
namespace Starcounter.Linq.QueryTests
{
    [Database]
    public class Department
    {
        private static readonly Func<Department, IEnumerable<Employee>> EmployeesByDepartment =
            CompileQuery((Department dep) => Objects<Employee>().Where(e => e.Department == dep));

        public Company Company { get; set; }
        public string Name { get; set; }

        public IEnumerable<Employee> Employees => EmployeesByDepartment(this);
        public bool Global { get; set; }
    }
}