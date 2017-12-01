using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Starcounter.Linq
{
    // ReSharper disable once InconsistentNaming
    public static class DbLinq
    {
        private static class Cache<T>
        {
            public static Queryable<T> Objects { get; } = new Queryable<T>(new QueryProvider(new QueryContext<T>()));
        }

        public static Queryable<T> Objects<T>() => Cache<T>.Objects;


        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<IEnumerable<TResult>> CompileQuery<TResult>(
            Expression<Func<IEnumerable<TResult>>> queryExpression)
            where TResult : class
            => new CompiledQuery<TResult>(queryExpression).Execute<IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<IEnumerable<TResult>> CompileQuery<TResult>(
            Expression<Func<IQueryable<TResult>>> queryExpression)
            => new CompiledQuery<TResult>(queryExpression).Execute<IEnumerable<TResult>>;


        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TResult> CompileQuery<TResult>(
            Expression<Func<TResult>> queryExpression)
            => new CompiledQuery<TResult>(queryExpression).Execute<TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, IEnumerable<TResult>> CompileQuery<TParam1, TResult>(
            Expression<Func<TParam1, IQueryable<TResult>>> queryExpression)
            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TResult> CompileQuery<TParam1, TResult>(
             Expression<Func<TParam1, TResult>> queryExpression)
            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TResult>(
             Expression<Func<TParam1, TParam2, IQueryable<TResult>>> queryExpression)

            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, IEnumerable<TResult>>;


        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, TResult> CompileQuery<
             TParam1, TParam2, TResult>(
             Expression<Func<TParam1, TParam2, TResult>> queryExpression)

            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, TParam3, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TParam3, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, IQueryable<TResult>>> queryExpression)

            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, TParam3, TResult> CompileQuery<
             TParam1, TParam2, TParam3, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TResult>> queryExpression)

            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, TParam3, TParam4, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, IQueryable<TResult>>> queryExpression)

            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, TParam3, TParam4, TResult> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, TResult>> queryExpression)

            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TParam5">The type of the fifth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, TParam3, TParam4, TParam5, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, TParam5, IQueryable<TResult>>> queryExpression)

            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, TParam5, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TParam5">The type of the fifth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        public static Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>> queryExpression)
            => new CompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>;
    }
}