using System.Linq;
using System.Reflection;
using Starcounter.Nova;

namespace Starcounter.Linq.Helpers
{
    internal static class ReflectionHelper
    {
        private static MethodInfo _dbSlowSqlMethodBase;

        public static MethodInfo DbSlowSqlMethodBase =>
            _dbSlowSqlMethodBase ??
            (_dbSlowSqlMethodBase = typeof(Db).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                              .Where(x => x.Name == nameof(Db.SlowSQL))
                                              .Single(x => x.IsGenericMethod));
    }
}
