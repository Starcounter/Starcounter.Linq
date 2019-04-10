﻿using System;
using System.Linq.Expressions;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq.Visitors
{
    internal class RootVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static readonly RootVisitor<TEntity> Instance = new RootVisitor<TEntity>();

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            var method = node.Method.IsGenericMethod ? node.Method.GetGenericMethodDefinition() : node.Method;
            if (method == KnownMethods<TEntity>.QueryableDeletePred)
            {
                state.ResultMethod = QueryResultMethod.Delete;
                var expression = node.Arguments[0];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<TEntity>.QueryableDeleteAll)
            {
                state.ResultMethod = QueryResultMethod.Delete;
            }
            else if (method == KnownMethods<TEntity>.QueryableFirstOrDefaultPred || method == KnownMethods<TEntity>.QueryableAnyPred)
            {
                state.ResultMethod = method == KnownMethods<TEntity>.QueryableFirstOrDefaultPred
                    ? QueryResultMethod.FirstOrDefault
                    : QueryResultMethod.Any;
                var expression = node.Arguments[0];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<TEntity>.QueryableFirstOrDefault || method == KnownMethods<TEntity>.QueryableAny)
            {
                state.ResultMethod = method == KnownMethods<TEntity>.QueryableFirstOrDefault
                    ? QueryResultMethod.FirstOrDefault
                    : QueryResultMethod.Any;
            }
            else if (method == KnownMethods.IQueryableFirstOrDefaultPred || method == KnownMethods.IQueryableFirstPred ||
                     method == KnownMethods.IQueryableSingleOrDefaultPred || method == KnownMethods.IQueryableSinglePred ||
                     method == KnownMethods.IQueryableAnyPred || method == KnownMethods.IQueryableAllPred)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                state.ResultMethod = method == KnownMethods.IQueryableFirstOrDefaultPred ? QueryResultMethod.FirstOrDefault
                    : method == KnownMethods.IQueryableFirstPred ? QueryResultMethod.First
                        : method == KnownMethods.IQueryableSingleOrDefaultPred ? QueryResultMethod.SingleOrDefault
                            : method == KnownMethods.IQueryableSinglePred ? QueryResultMethod.Single
                                : method == KnownMethods.IQueryableAnyPred ? QueryResultMethod.Any
                                    : QueryResultMethod.All;
                var expression = node.Arguments[1];
                if (state.ResultMethod == QueryResultMethod.All)
                {
                    state.AllMethodExpression = expression;
                }
                else
                {
                    VisitWhere(expression, state);
                }
            }
            else if (method == KnownMethods.IQueryableFirstOrDefault || method == KnownMethods.IQueryableFirst ||
                     method == KnownMethods.IQueryableSingleOrDefault || method == KnownMethods.IQueryableSingle ||
                     method == KnownMethods.IQueryableAny)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                state.ResultMethod = method == KnownMethods.IQueryableFirstOrDefault ? QueryResultMethod.FirstOrDefault
                    : method == KnownMethods.IQueryableFirst ? QueryResultMethod.First
                        : method == KnownMethods.IQueryableSingleOrDefault ? QueryResultMethod.SingleOrDefault
                            : method == KnownMethods.IQueryableSingle ? QueryResultMethod.Single
                                : QueryResultMethod.Any;
            }
            else if (method == KnownMethods.IQueryableWhere)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods.IQueryableCountPredicate)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
                state.SetAggregation(AggregationOperation.Count);
            }
            else if (method == KnownMethods.IQueryableTake)
            {
                if (node.Arguments[1] is ConstantExpression takeConstExpression)
                {
                    var value = (int)takeConstExpression.Value;
                    state.Fetch(value);
                    Visit(node.Arguments[0], state);
                }
                else if (node.Arguments[1] is MemberExpression takeMemberExpression)
                {
                    var value = (int)takeMemberExpression.RetrieveMemberValue();
                    state.Fetch(value);
                    Visit(node.Arguments[0], state);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else if (method == KnownMethods.IQueryableSkip)
            {
                if (node.Arguments[1] is ConstantExpression skipConstExpression)
                {
                    var value = (int)skipConstExpression.Value;
                    state.Offset(value);
                    Visit(node.Arguments[0], state);
                }
                else if (node.Arguments[1] is MemberExpression takeMemberExpression)
                {
                    var value = (int)takeMemberExpression.RetrieveMemberValue();
                    state.Offset(value);
                    Visit(node.Arguments[0], state);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else if (method.IsGenericMethod)
            {
                var gen = method.GetGenericMethodDefinition();

                //Why cannot this be done like the above?
                //Because OrderBy use Func<T,TKeySelector> where TKeySelector is unknown to us here
                //That is, it is the return type of the property
                if (gen == KnownMethods.IQueryableOrderBy)
                {
                    VisitOrderBy(node, state, true);
                }
                else if (gen == KnownMethods.IQueryableOrderByDesc)
                {
                    VisitOrderBy(node, state, false);
                }
                else if (gen == KnownMethods.IQueryableThenBy)
                {
                    VisitOrderBy(node, state, true);
                }
                else if (gen == KnownMethods.IQueryableThenByDesc)
                {
                    VisitOrderBy(node, state, false);
                }
                else if (gen == KnownMethods.IQueryableSelect)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                }
                else if (gen == KnownMethods.IQueryableWhere)
                {
                    var left = node.Arguments[0];
                    var expression = node.Arguments[1];
                    Visit(left, state);
                    VisitWhere(expression, state);
                }
                else if (gen == KnownMethods.IQueryableGroupBy)
                {
                    VisitGroupBy(node, state);
                }
                else if (gen == KnownMethods.IQueryableCount)
                {
                    var left = node.Arguments[0];
                    Visit(left, state);
                    state.SetAggregation(AggregationOperation.Count);
                }
                else if (gen == KnownMethods.IQueryableAverage)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.SetAggregation(AggregationOperation.Average);
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                }
                else if (gen == KnownMethods.IQueryableMin)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.SetAggregation(AggregationOperation.Min);
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                }
                else if (gen == KnownMethods.IQueryableMax)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.SetAggregation(AggregationOperation.Max);
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                }
                else if (gen == KnownMethods.IQueryableSum)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.SetAggregation(AggregationOperation.Sum);
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                }
            }
        }

        private void VisitOrderBy(MethodCallExpression node, QueryBuilder<TEntity> state, bool asc)
        {
            var left = node.Arguments[0];
            Visit(left, state);
            state.BeginOrderBySection();
            var arg = node.Arguments[1];
            OrderByVisitor<TEntity>.Instance.Visit(arg, state);
            state.EndOrderBySection(asc);
        }

        private void VisitGroupBy(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            var left = node.Arguments[0];
            Visit(left, state);
            var arg = node.Arguments[1];
            GroupByVisitor<TEntity>.Instance.Visit(arg, state);
        }

        private static void VisitWhere(Expression expression, QueryBuilder<TEntity> state)
        {
            state.BeginWhereSection();
            WhereVisitor<TEntity>.Instance.Visit(expression, state);
            state.EndWhereSection();
        }

        public override void VisitLambda(LambdaExpression node, QueryBuilder<TEntity> state)
        {
            var arg = node.Parameters[0];

            WhereVisitor<TEntity>.Instance.Visit(node.Body, state);
        }
    }
}
