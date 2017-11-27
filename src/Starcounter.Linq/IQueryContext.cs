using System.Linq.Expressions;

namespace Starcounter.Linq
{
    public interface IQueryContext
    {
        object Execute(Expression expression);
        object Execute<TResult>(Expression expression);
        string GetQuery(Expression expression);
    }
}