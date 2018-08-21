using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter.Nova;
using static Starcounter.Linq.DbLinq;

// ReSharper disable once CheckNamespace
namespace Starcounter.Linq.QueryTests
{
    [Database]
    public abstract class Company : INamed
    {
        private static readonly Func<Company, IEnumerable<Department>> DepartmentsByCompany =
            CompileQuery((Company com) => Objects<Department>().Where(e => e.Company == com));

        public abstract string Name { get; set; }
        public abstract bool Global { get; set; }
        public IEnumerable<Department> Departments => DepartmentsByCompany(this);
    }
}