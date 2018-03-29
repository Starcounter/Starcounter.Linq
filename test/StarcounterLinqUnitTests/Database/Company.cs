using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter.Linq;
using Starcounter.Nova;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public abstract class Company
    {
        private static readonly Func<Company, IEnumerable<Department>> DepartmentsByCompany =
            DbLinq.CompileQuery((Company com) =>
                DbLinq.Objects<Department>().Where(e => e.Company == com));

        public abstract string Name { get; set; }
        public abstract bool Global { get; set; }
        public IEnumerable<Department> Departments => DepartmentsByCompany(this);
    }
}