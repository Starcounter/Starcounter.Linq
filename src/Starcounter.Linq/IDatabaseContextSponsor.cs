using Starcounter.Database;

namespace Starcounter.Linq
{
    public static class IDatabaseContextSponsor
    {
        private static class Cache<T>
        {
            public static Queryable<T> GetObjects(IDatabaseContext db) 
                => new Queryable<T>(new QueryProvider(new QueryContext<T>(new QueryExecutor<T>(db))));
        }

        public static Queryable<T> Objects<T>(this IDatabaseContext db) => Cache<T>.GetObjects(db);
    }
}