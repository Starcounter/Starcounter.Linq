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
            public static Queryable<T> Objects { get; } = new Queryable<T>(new QueryProvider(new QueryContext<T>(new QueryExecutor<T>())));
        }

        /// <summary>
        /// Can be used for building LINQ expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        ///<remarks>
        /// The LINQ expression is translated to SQL every time it's called.
        /// This is an expensive operation. Thus, don't use it in places where it's executed many times.
        /// Instead, use compiled query.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// </remarks>
        public static Queryable<T> Objects<T>() => Cache<T>.Objects;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<IEnumerable<TResult>> CompileQuery<TResult>(
            Expression<Func<IEnumerable<TResult>>> queryExpression)
            where TResult : class
            => CreateCompiledQuery<TResult>(queryExpression).Execute<IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<IEnumerable<TResult>> CompileQuery<TResult>(
            Expression<Func<IQueryable<TResult>>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<IEnumerable<TResult>> CompileQuery<TResult>(
            Expression<Func<IOrderedQueryable<TResult>>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TResult> CompileQuery<TResult>(
            Expression<Func<TResult>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, IEnumerable<TResult>> CompileQuery<TParam1, TResult>(
            Expression<Func<TParam1, IQueryable<TResult>>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TResult> CompileQuery<TParam1, TResult>(
             Expression<Func<TParam1, TResult>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TResult>(
             Expression<Func<TParam1, TParam2, IQueryable<TResult>>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, TResult> CompileQuery<
             TParam1, TParam2, TResult>(
             Expression<Func<TParam1, TParam2, TResult>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, TParam3, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TParam3, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, IQueryable<TResult>>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, TParam3, TResult> CompileQuery<
             TParam1, TParam2, TParam3, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TResult>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, TParam3, TParam4, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, IQueryable<TResult>>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, TParam3, TParam4, TResult> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, TResult>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, TResult>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TParam5">The type of the fifth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, TParam3, TParam4, TParam5, IEnumerable<TResult>> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, TParam5, IQueryable<TResult>>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, TParam5, IEnumerable<TResult>>;

        /// <summary>
        /// Creates a compiled query delegate that when invoked will execute the specified LINQ query.
        /// It lets you build a LINQ expression with translated SQL once and use it many times.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first query parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second query parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third query parameter.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth query parameter.</typeparam>
        /// <typeparam name="TParam5">The type of the fifth query parameter.</typeparam>
        /// <typeparam name="TResult">The query result type.</typeparam>
        /// <param name="queryExpression">The LINQ query expression.</param>
        /// <returns>A delegate that can be invoked to execute the compiled query.</returns>
        /// <remarks>
        /// Use it in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.
        /// <br/>Restrictions:
        /// <br/> - Starcounter.Linq only supports database properties. It is not possible to get access to fields.
        /// <br/> - Starcounter.Linq uses literal values for FETCH and OFFSET clauses for performance reason, it means that you cannot pass the value when executing a compiled query.
        /// <br/> - Since comparisons with null values are translated to IS NULL form in SQL, there is no possibility to pass such values with parameters into compiled queries.
        /// <br/> - The Contains method is not supported by compiled queries.
        /// </remarks>
        public static Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> CompileQuery<
             TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
             Expression<Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>> queryExpression)
            => CreateCompiledQuery<TResult>(queryExpression).Execute<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>;

        private static CompiledQuery<T> CreateCompiledQuery<T>(LambdaExpression expression)
        {
            return new CompiledQuery<T>(expression, new QueryExecutor<T>());
        }
    }
}