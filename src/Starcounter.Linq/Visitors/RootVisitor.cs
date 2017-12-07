using System;
using System.Linq.Expressions;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq.Visitors
{
    internal class RootVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static readonly RootVisitor<TEntity> Instance = new RootVisitor<TEntity>();

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            var method = node.Method;
            if (method == KnownMethods<TEntity>.QueryableFirstOrDefaultPred || method == KnownMethods<TEntity>.QueryableAnyPred)
            {
                state.ResultMethod = method == KnownMethods<TEntity>.QueryableFirstOrDefaultPred
                    ? QueryResultMethod.FirstOrDefault
                    : QueryResultMethod.Any;
                var expression = node.Arguments[0];
                if (state.ResultMethod == QueryResultMethod.All)
                {
                    state.AllMethodExpression = expression;
                }
                else
                {
                    VisitWhere(expression, state);
                }
            }
            else if (method == KnownMethods<TEntity>.QueryableFirstOrDefault || method == KnownMethods<TEntity>.QueryableAny)
            {
                state.ResultMethod = method == KnownMethods<TEntity>.QueryableFirstOrDefault
                    ? QueryResultMethod.FirstOrDefault
                    : QueryResultMethod.Any;
            }
            else if (method == KnownMethods<TEntity>.IQueryableFirstOrDefaultPred || method == KnownMethods<TEntity>.IQueryableFirstPred ||
                     method == KnownMethods<TEntity>.IQueryableSingleOrDefaultPred || method == KnownMethods<TEntity>.IQueryableSinglePred ||
                     method == KnownMethods<TEntity>.IQueryableAnyPred || method == KnownMethods<TEntity>.IQueryableAllPred)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                state.ResultMethod = method == KnownMethods<TEntity>.IQueryableFirstOrDefaultPred ? QueryResultMethod.FirstOrDefault
                    : method == KnownMethods<TEntity>.IQueryableFirstPred ? QueryResultMethod.First
                        : method == KnownMethods<TEntity>.IQueryableSingleOrDefaultPred ? QueryResultMethod.SingleOrDefault
                            : method == KnownMethods<TEntity>.IQueryableSinglePred ? QueryResultMethod.Single
                                : method == KnownMethods<TEntity>.IQueryableAnyPred ? QueryResultMethod.Any
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
            else if (method == KnownMethods<TEntity>.IQueryableFirstOrDefault || method == KnownMethods<TEntity>.IQueryableFirst ||
                     method == KnownMethods<TEntity>.IQueryableSingleOrDefault || method == KnownMethods<TEntity>.IQueryableSingle ||
                     method == KnownMethods<TEntity>.IQueryableAny)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                state.ResultMethod = method == KnownMethods<TEntity>.IQueryableFirstOrDefault ? QueryResultMethod.FirstOrDefault
                    : method == KnownMethods<TEntity>.IQueryableFirst ? QueryResultMethod.First
                        : method == KnownMethods<TEntity>.IQueryableSingleOrDefault ? QueryResultMethod.SingleOrDefault
                            : method == KnownMethods<TEntity>.IQueryableSingle ? QueryResultMethod.Single
                                : QueryResultMethod.Any;
            }
            else if (method == KnownMethods<TEntity>.IQueryableWhere)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<TEntity>.IQueryableCountPredicate)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
                state.WriteSelect($"COUNT({state.GetSourceName()})");
            }
            else if (method == KnownMethods<TEntity>.IQueryableTake)
            {
                if (node.Arguments[1] is ConstantExpression takeConstExpression)
                {
                    var value = (int)takeConstExpression.Value;
                    state.Fetch(value);
                    Visit(node.Arguments[0], state);
                }
                else if (node.Arguments[1] is MemberExpression takeMemberExpression)
                {
                    var value = (int)takeMemberExpression.RetrieveValue();
                    state.Fetch(value);
                    Visit(node.Arguments[0], state);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else if (method == KnownMethods<TEntity>.IQueryableSkip)
            {
                if (node.Arguments[1] is ConstantExpression skipConstExpression)
                {
                    var value = (int)skipConstExpression.Value;
                    state.Offset(value);
                    Visit(node.Arguments[0], state);
                }
                else if (node.Arguments[1] is MemberExpression takeMemberExpression)
                {
                    var value = (int)takeMemberExpression.RetrieveValue();
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
                else if (gen == KnownMethods.IQueryableCount)
                {
                    var left = node.Arguments[0];
                    Visit(left, state);
                    state.WriteSelect($"COUNT({state.GetSourceName()})");
                }
                else if (gen == KnownMethods.IQueryableAverage)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("AVG(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
                }
                else if (gen == KnownMethods.IQueryableMin)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("MIN(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
                }
                else if (gen == KnownMethods.IQueryableMax)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("MAX(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
                }
                else if (gen == KnownMethods.IQueryableSum)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("SUM(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
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
