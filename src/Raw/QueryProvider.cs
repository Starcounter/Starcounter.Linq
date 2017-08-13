using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Starcounter.Linq.Raw
{
    public class QueryProvider : IQueryProvider
    {
        private readonly IQueryContext _queryContext;

        public QueryProvider(IQueryContext queryContext)
        {
            _queryContext = queryContext;
        }

        public virtual IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable) Activator.CreateInstance(typeof(Queryable<>).MakeGenericType(elementType), this,
                    expression);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public virtual IQueryable<T> CreateQuery<T>(Expression expression)
        {
            return new Queryable<T>(this, expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return _queryContext.Execute(expression);
        }

        T IQueryProvider.Execute<T>(Expression expression)
        {
            return (T) _queryContext.Execute(expression);
        }
    }
}