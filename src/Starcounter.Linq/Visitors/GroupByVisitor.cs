using System.Linq.Expressions;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq.Visitors
{
    internal class GroupByVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static GroupByVisitor<TEntity> Instance = new GroupByVisitor<TEntity>();

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Expression is ParameterExpression)
                state.WriteGroupBy(state.GetSource());
            else
                Visit(node.Expression, state);
            state.WriteGroupBy("." + SqlHelper.EscapeSingleIdentifier(node.Member.Name));
        }

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Method == KnownMethods.GetOid)
            {
                state.WriteGroupByObjectNo();
            }
            else
            {
                base.VisitMethodCall(node, state);
            }
        }
    }
}
