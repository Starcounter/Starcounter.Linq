using System.Linq;

namespace PoS.Infra
{
    public static class DbLinq
    {
        public static IQueryable<T> Objects<T>()
        {
            return new SqlQueryable<T>();
        }
    }
}