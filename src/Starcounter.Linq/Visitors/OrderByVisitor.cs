using System.Linq.Expressions;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq.Visitors
{
    internal class OrderByVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static OrderByVisitor<TEntity> Instance = new OrderByVisitor<TEntity>();

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Expression is ParameterExpression)
                state.WriteOrderBy(state.GetSource());
            else
                Visit(node.Expression, state);
            state.WriteOrderBy("." + SqlHelper.EscapeSingleIdentifier(node.Member.Name));
        }

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Method == KnownMethods.GetObjectNo)
            {
                state.WriteOrderByObjectNo();
            }
            else
            {
                base.VisitMethodCall(node, state);
            }
        }
    }
}