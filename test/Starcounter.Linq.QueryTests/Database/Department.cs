using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter.Nova;
using static Starcounter.Linq.DbLinq;

// ReSharper disable once CheckNamespace
namespace Starcounter.Linq.QueryTests
{
    [Database]
    public abstract class Department : INamed, IHaveCompany
    {
        private static readonly Func<Department, IEnumerable<Employee>> EmployeesByDepartment =
            CompileQuery((Department dep) => Objects<Employee>().Where(e => e.Department == dep));

        public abstract Company Company { get; set; }
        public abstract string Name { get; set; }

        public IEnumerable<Employee> Employees => EmployeesByDepartment(this);
        public abstract bool Global { get; set; }
    }
}