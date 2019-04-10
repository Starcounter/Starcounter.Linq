using System.Linq.Expressions;
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
            state.AppendSelectPath("." + SqlHelper.EscapeSingleIdentifier(node.Member.Name));
        }

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Method == KnownMethods<TEntity>.EnumerableCount)
            {
                state.SetAggregation(AggregationOperation.Count);
            }
            else if (node.Method == KnownMethods<TEntity>.EnumerableMax)
            {
                state.SetAggregation(AggregationOperation.Max);
            }
            else if (node.Method == KnownMethods<TEntity>.EnumerableMin)
            {
                state.SetAggregation(AggregationOperation.Min);
            }
            else if (node.Method == KnownMethods<TEntity>.EnumerableCountPred)
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
            else if (node.Method == KnownMethods<TEntity>.EnumerableMaxTarget)
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Max);
                Visit(right, state);
            }
            else if (node.Method == KnownMethods<TEntity>.EnumerableMinTarget)
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Min);
                Visit(right, state);
            }
            else if (node.Method == KnownMethods<TEntity>.EnumerableSum)
            {
                var right = node.Arguments[1];
                state.SetAggregation(AggregationOperation.Sum);
                Visit(right, state);
            }
            else if (node.Method == KnownMethods<TEntity>.EnumerableAverage)
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
    }
}
