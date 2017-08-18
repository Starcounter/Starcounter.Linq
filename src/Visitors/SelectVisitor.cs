using System.Linq.Expressions;

namespace Starcounter.Linq.Visitors
{
    public class SelectVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static OrderByVisitor<TEntity> Instance = new OrderByVisitor<TEntity>();

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Expression is ParameterExpression param)
                state.WriteSelect(param.Type.SourceName());
            else
                Visit(node.Expression, state);
            state.WriteSelect("." + node.Member.Name);
        }
    }
}
