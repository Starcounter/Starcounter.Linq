using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Starcounter.Linq.Tests
{
    public static class Utils
    {
        public static string Sql<T>(Expression<Func<IEnumerable<T>>> exp) 
        {
            return new CompiledQuery<T>(exp).SqlStatement;
        }
        public static string Sql<T>(Expression<Func<IQueryable<T>>> exp)
        {
            return new CompiledQuery<T>(exp).SqlStatement;
        }
        public static string Sql<T>(Expression<Func<IOrderedQueryable<T>>> exp)
        {
            return new CompiledQuery<T>(exp).SqlStatement;
        }
        public static string Sql<T>(Expression<Func<T>> exp) 
        {
            return new CompiledQuery<T>(exp).SqlStatement;
        }
    }
}
