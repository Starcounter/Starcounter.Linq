using System;
using System.Linq;
using System.Linq.Expressions;

namespace Starcounter.Linq.Tests
{
    public static class Utils
    {
        public static string Sql<T>(Expression<Func<IQueryable<T>>> exp) => new CompiledQuery<T>(exp).SqlStatement;

        public static string Sql<T>(Expression<Func<T>> exp) => new CompiledQuery<T>(exp).SqlStatement;

        // TODO temporary
        public static string Sql<T, TResult>(Expression<Func<TResult>> exp)
            where TResult : struct
            => new CompiledQuery<T>(exp).SqlStatement;
    }
}
