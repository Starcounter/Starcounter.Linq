namespace Starcounter.Linq
{
    public interface IQueryExecutor
    {
        object Execute<TResult>(string sql, object[] variables, QueryResultMethod queryResultMethod);
    }
}