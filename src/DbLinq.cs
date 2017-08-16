namespace Starcounter.Linq
{

    public partial class DbLinq
    {
        private static class Cache<T>
        {
            public static Queryable<T> Objects { get; } = new Queryable<T>(new QueryProvider(new QueryContext<T>()));
        }
        public static Queryable<T> Objects<T>() => Cache<T>.Objects;
    }
    public static class DummyLinq
    {
        private static class Cache<T>
        {
            public static Queryable<T> Objects { get; } = new Queryable<T>(new QueryProvider(new DummyQueryContext<T>()));
        }

        public static Queryable<T> Objects<T>() => Cache<T>.Objects;
    }
}