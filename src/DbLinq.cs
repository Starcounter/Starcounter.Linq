using System;
using System.Collections.Generic;
using System.Linq;

namespace Starcounter.Linq
{
    public static class DbLinq
    {
        public static IQueryable<T> Objects<T>() => new SqlQueryable<T>(Shared.Executor);

        public static IQueryable<T> EmptyObjects<T>() => new SqlQueryable<T>(Shared.EmptyExecutor);
    }
}