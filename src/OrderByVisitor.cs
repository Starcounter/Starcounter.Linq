using System.Linq.Expressions;

namespace Starcounter.Linq
{
    public class OrderByVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static OrderByVisitor<TEntity> Instance = new OrderByVisitor<TEntity>();

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Expression is ParameterExpression param)
                state.WriteOrderBy(param.Type.SourceName());
            else
                Visit(node.Expression, state);
            state.WriteOrderBy("." + node.Member.Name);
        }
    }
}