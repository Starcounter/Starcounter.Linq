using System;
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
    public class CompiledQuery<T>
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TResult>() => ExecuteCore<TResult>();

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TResult>(TParam1 param1)
            => ExecuteCore<TResult>(param1);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult ExecuteAsync<TParam1, TResult>(
            CancellationToken cancellationToken,
            TParam1 param1) => ExecuteCore<TResult>(param1);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2, TResult>(
            TParam1 param1,
            TParam2 param2) => ExecuteCore<TResult>(param1, param2);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2, TParam3, TResult>(
            TParam1 param1,
            TParam2 param2,
            TParam3 param3) => ExecuteCore<TResult>(param1, param2, param3);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2, TParam3, TParam4, TResult>(
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4) => ExecuteCore<TResult>(param1, param2, param3, param4);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TResult Execute<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4,
            TParam5 param5) => ExecuteCore<TResult>(param1, param2, param3, param4, param5);

        public string SqlStatement { get; }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public CompiledQuery(LambdaExpression queryExpression)
        {
            Type contextType;
            switch (queryExpression.Body)
            {
                case MethodCallExpression methodCall:
                    contextType = (methodCall.Arguments[0] as MethodCallExpression)?.Type.GenericTypeArguments[0];
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (contextType == null)
            {
                var q = new DummyQueryContext<T>();
                SqlStatement = q.GetQuery(queryExpression.Body);
            }
            else
            {
                var queryContextType = typeof(DummyQueryContext<>).MakeGenericType(contextType);
                object q = Activator.CreateInstance(queryContextType);
                var methodInfo = queryContextType.GetMethod(nameof(DummyQueryContext<object>.GetQuery));
                if (methodInfo == null)
                {
                    throw new MissingMethodException();
                }
                SqlStatement = methodInfo.Invoke(q, new object[] { queryExpression.Body }) as string;
            }
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual TResult ExecuteCore<TResult>(params object[] parameters) => ExecuteCore<TResult>(CancellationToken.None, parameters);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual TResult ExecuteCore<TResult>(CancellationToken cancellationToken, params object[] parameters)
        {
            if (typeof(TResult).IsGenericType)
            {
                return (TResult)(object)Db.SlowSQL<T>(SqlStatement, parameters);
            }

            var result = Db.SlowSQL(SqlStatement, parameters).FirstOrDefault();
            if (result == null)
            {
                return default(TResult);
            }
            var resultType = result.GetType();

            // SC lifts underlying types to a bigger ones in some cases.
            // Look at the issue https://github.com/Starcounter/Home/issues/209 for getting more info.
            if (resultType != typeof(TResult) && !resultType.IsSubclassOf(typeof(TResult)))
            {
                var expectedType = typeof(TResult);
                if (expectedType == typeof(int))
                {
                    result = Convert.ToInt32(result);
                }
                if (expectedType == typeof(long))
                {
                    result = Convert.ToInt64(result);
                }
                if (expectedType == typeof(decimal))
                {
                    result = Convert.ToDecimal(result);
                }
                if (expectedType == typeof(double))
                {
                    result = Convert.ToDouble(result);
                }
                if (expectedType == typeof(uint))
                {
                    result = Convert.ToUInt32(result);
                }
                if (expectedType == typeof(ulong))
                {
                    result = Convert.ToUInt64(result);
                }
                if (expectedType == typeof(bool))
                {
                    result = Convert.ToBoolean(result);
                }
                if (expectedType == typeof(string))
                {
                    result = Convert.ToString(result);
                }
                if (expectedType == typeof(float))
                {
                    result = Convert.ToSingle(result);
                }
            }
            return (TResult)result;
        }
    }
}