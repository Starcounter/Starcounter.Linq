using System;
using System.Collections.Generic;
using System.Linq;
using static Starcounter.Linq.DbLinq;

// ReSharper disable once CheckNamespace
namespace Starcounter.Linq.QueryTests
{
    [Database]
    public class Company
    {
        private static readonly Func<Company, IEnumerable<Department>> DepartmentsByCompany =
            CompileQuery((Company com) => Objects<Department>().Where(e => e.Company == com));

        public string Name { get; set; }
        public bool Global { get; set; }
        public IEnumerable<Department> Departments => DepartmentsByCompany(this);
    }
}