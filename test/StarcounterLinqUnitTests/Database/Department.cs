using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public abstract class Department
    {
        private static readonly Func<Department, IEnumerable<Employee>> EmployeesByDepartment =
            DbLinq.CompileQuery((Department dep) =>
                DbLinq.Objects<Employee>().Where(e => e.Department == dep));

        public abstract Company Company { get; set; }
        public abstract string Name { get; set; }

        public IEnumerable<Employee> Employees => EmployeesByDepartment(this);
        public abstract bool Global { get; set; }
    }
}