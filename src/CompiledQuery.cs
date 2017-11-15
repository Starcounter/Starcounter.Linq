using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Starcounter.Linq
{
    // Copyright (c) .NET Foundation. All rights reserved.
    // Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class CompiledQuery<TResult>
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute() => ExecuteCore();

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1>(
            TParam1 param1) => ExecuteCore(param1);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult ExecuteAsync<TParam1>(
            CancellationToken cancellationToken,
            TParam1 param1) => ExecuteCore(param1);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2>(
            TParam1 param1,
            TParam2 param2) => ExecuteCore(param1, param2);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2, TParam3>(
            TParam1 param1,
            TParam2 param2,
            TParam3 param3) => ExecuteCore(param1, param2, param3);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2, TParam3, TParam4>(
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4) => ExecuteCore(param1, param2, param3, param4);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2, TParam3, TParam4, TParam5>(
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4,
            TParam5 param5) => ExecuteCore(param1, param2, param3, param4, param5);

        public string SqlStatement { get; }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public CompiledQuery(LambdaExpression queryExpression)
        {
            var q = new DummyQueryContext<TResult>();
            SqlStatement = q.GetQuery(queryExpression.Body);
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual TResult ExecuteCore(params object[] parameters) => ExecuteCore(CancellationToken.None, parameters);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual TResult ExecuteCore(CancellationToken cancellationToken, params object[] parameters)
        {
            if (typeof(TResult).IsGenericType)
            {
                return (TResult)(object)Db.SQL<TResult>(SqlStatement, parameters);
            }
            return Db.SlowSQL<TResult>(SqlStatement, parameters).FirstOrDefault();
        }
    }
}