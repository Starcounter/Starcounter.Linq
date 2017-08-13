using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Starcounter.Linq.Raw;

namespace Starcounter.Linq.Cache
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public abstract class CompiledQueryBase<TResult>
    {
        private readonly LambdaExpression _queryExpression;

        private readonly Func<TResult> _executor;
        private readonly string _sqlStatement;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected CompiledQueryBase(LambdaExpression queryExpression)
        {
            var q = new QueryContext<TResult>();
            _sqlStatement = q.GetQuery(queryExpression.Body);
            _queryExpression = queryExpression;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual TResult ExecuteCore(

            params object[] parameters)
            => ExecuteCore(default(CancellationToken), parameters);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual TResult ExecuteCore(

            CancellationToken cancellationToken,
            params object[] parameters)
        {
            if (typeof(TResult).IsGenericType)
            {
                return (TResult)(object)Db.SQL<TResult>(_sqlStatement, parameters);
            }
            return Db.SQL<TResult>(_sqlStatement, parameters).First;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected abstract Func<TResult> CreateCompiledQuery(Expression expression);
    }
}


