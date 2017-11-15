using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter;
using Starcounter.Linq;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public class Company
    {
        private static readonly Func<Company, IEnumerable<Department>> DepartmentsByCompany =
            DbLinq.CompileQuery((Company com) =>
                DbLinq.Objects<Department>().Where(e => e.Company == com));

        public string Name { get; set; }

        public IEnumerable<Department> Departments => DepartmentsByCompany(this);
    }
}