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
    }
}
