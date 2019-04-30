using System.Linq.Expressions;
using System.Reflection;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq.Visitors
{
    internal class SelectVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static SelectVisitor<TEntity> Instance = new SelectVisitor<TEntity>();

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            if (!(node.Expression is ParameterExpression))
            {
                Visit(node.Expression, state);
            }
            state.AppendSelectTarget(node.Member.Name);
        }

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            MethodInfo method = node.Method.IsGenericMethod ? node.Method.GetGenericMethodDefinition() : node.Method;
            if (method == KnownMethods.EnumerableCount || method == KnownMethods.EnumerableLongCount)
            {
                state.SetAggregation(AggregationOperation.Count);
            }
            else if (method == KnownMethods.EnumerableMaxDirect)
            {
                state.SetAggregation(AggregationOperation.Max);
            }
            else if (method == KnownMethods.EnumerableMinDirect)
            {
                state.SetAggregation(AggregationOperation.Min);
            }
            else if (method == KnownMethods.EnumerableCountPredicate || method == KnownMethods.EnumerableLongCountPredicate)
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Count);
                if (right is LambdaExpression lambda)
                {
                    state.BeginWhereSection();
                    WhereVisitor<TEntity>.Instance.Visit(lambda.Body, state);
                    state.EndWhereSection();
                }
            }
            else if (KnownMethods.IsEnumerableMax(method))
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Max);
                Visit(right, state);
            }
            else if (KnownMethods.IsEnumerableMin(method))
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Min);
                Visit(right, state);
            }
            else if (KnownMethods.IsEnumerableSum(method))
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Sum);
                Visit(right, state);
            }
            else if (KnownMethods.IsEnumerableAverage(method))
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Average);
                Visit(right, state);
            }
            else
            {
                base.VisitMethodCall(node, state);
            }
        }

        public override void VisitNew(NewExpression node, QueryBuilder<TEntity> state)
        {
            state.BeginSelectSection(node.Constructor);
            base.VisitNew(node, state);
            state.EndSelectSection();
        }
    }
}
