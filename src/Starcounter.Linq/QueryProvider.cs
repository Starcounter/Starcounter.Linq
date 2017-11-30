using System.Linq;
using System.Linq.Expressions;

namespace Starcounter.Linq
{
    public class QueryProvider : IQueryProvider
    {
        private readonly IQueryContext QueryContext;

        public QueryProvider(IQueryContext queryContext)
        {
            QueryContext = queryContext;
        }

        //Multi-method magic, this is faster than `MakeGenericType` + Activator.CreateInstance
        public virtual IQueryable CreateQuery(Expression expression) => CreateQuery<dynamic>(expression);

        public virtual IQueryable<T> CreateQuery<T>(Expression expression) => new Queryable<T>(this, expression);

        object IQueryProvider.Execute(Expression expression) => QueryContext.Execute(expression);

        T IQueryProvider.Execute<T>(Expression expression) => (T)QueryContext.Execute<T>(expression);
    }
}