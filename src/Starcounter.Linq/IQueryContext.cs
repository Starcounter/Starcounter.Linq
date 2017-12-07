using System.Linq.Expressions;

namespace Starcounter.Linq
{
    public interface IQueryContext
    {
        object Execute(Expression expression);
        object Execute<TResult>(Expression expression);
        TranslatedQuery GetQuery(Expression expression);
    }
}