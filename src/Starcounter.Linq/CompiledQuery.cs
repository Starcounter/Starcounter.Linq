﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Starcounter.Linq
{
    // Copyright (c) .NET Foundation. All rights reserved.
    // Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

    public class CompiledQuery<T>
    {
        public IQueryExecutor QueryExecutor { get; }

        public string SqlStatement { get; }
        public QueryResultMethod QueryResultMethod { get; }

        /// <summary>
        /// This API supports the Entity Framework Core infrastructure and is not intended to be used
        /// directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public CompiledQuery(LambdaExpression queryExpression, IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));

            Type contextType;
            if (queryExpression.Body is MethodCallExpression methodCall)
            {
                var rootMethodCall = GetRootMethodCall(methodCall);
                if (methodCall != rootMethodCall)
                {
                    contextType = rootMethodCall.Type.GenericTypeArguments[0];
                }
                else
                {
                    contextType = rootMethodCall.Object?.Type.GenericTypeArguments.FirstOrDefault() ?? rootMethodCall.Type;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            TranslatedQuery translatedQuery;
            if (contextType == null)
            {
                var q = new DummyQueryContext<T>();
                translatedQuery = q.GetQuery(queryExpression.Body);
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
                translatedQuery = (TranslatedQuery)methodInfo.Invoke(q, new object[] { queryExpression.Body });
            }
            SqlStatement = translatedQuery.SqlStatement;
            QueryResultMethod = translatedQuery.ResultMethod;
        }

        private MethodCallExpression GetRootMethodCall(MethodCallExpression methodCall)
        {
            MethodCallExpression currentMethodCall = methodCall;
            while (currentMethodCall.Arguments.Any() && currentMethodCall.Arguments[0] is MethodCallExpression upperMethodCall)
            {
                currentMethodCall = upperMethodCall;
            }
            return currentMethodCall;
        }

        protected virtual TResult ExecuteCore<TResult>(CancellationToken cancellationToken, params object[] parameters)
        {
            if (parameters.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(parameters), "Compiled query does not support null value as a parameter.");
            }
            return (TResult)QueryExecutor.Execute<TResult>(SqlStatement, parameters, QueryResultMethod);
        }

        protected virtual TResult ExecuteCore<TResult>(params object[] parameters)
            => ExecuteCore<TResult>(CancellationToken.None, parameters);

        public virtual TResult Execute<TResult>() => ExecuteCore<TResult>();

        public virtual TResult Execute<TParam1, TResult>(TParam1 param1)
            => ExecuteCore<TResult>(param1);

        public virtual TResult ExecuteAsync<TParam1, TResult>(CancellationToken cancellationToken, TParam1 param1)
            => ExecuteCore<TResult>(param1);

        public virtual TResult Execute<TParam1, TParam2, TResult>(TParam1 param1, TParam2 param2)
            => ExecuteCore<TResult>(param1, param2);

        public virtual TResult Execute<TParam1, TParam2, TParam3, TResult>(
            TParam1 param1, TParam2 param2, TParam3 param3)
            => ExecuteCore<TResult>(param1, param2, param3);

        public virtual TResult Execute<TParam1, TParam2, TParam3, TParam4, TResult>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
            => ExecuteCore<TResult>(param1, param2, param3, param4);

        public virtual TResult Execute<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
            => ExecuteCore<TResult>(param1, param2, param3, param4, param5);
    }
}