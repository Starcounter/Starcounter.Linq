using System.Linq;

namespace Starcounter.Linq
{
    public static class DbLinq
    {
        public static IQueryable<T> Objects<T>()
        {
            return new SqlQueryable<T>();
        }
    }
}