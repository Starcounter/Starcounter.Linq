using System;
using System.Linq.Expressions;
using Starcounter.Linq.Visitors;

namespace Starcounter.Linq
{
    public class QueryContext<T> : IQueryContext
    {
        public IQueryExecutor QueryExecutor { get; }

        public QueryContext(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
        }

        public object Execute(Expression expression) => Execute<T>(expression);

        public object Execute<TResult>(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            string sql = query.BuildSqlString();
            object[] variables = query.GetVariables();

            return QueryExecutor.Execute<TResult>(sql, variables, query.ResultMethod, query.SelectComposerConstructor);
        }

        public TranslatedQuery GetQuery(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            return new TranslatedQuery
            {
                SqlStatement = sql,
                ResultMethod = query.ResultMethod,
                MultiTargetsConstructor = query.SelectComposerConstructor
            };
        }
    }
}
