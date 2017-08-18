using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType
// ReSharper disable InconsistentNaming

namespace Starcounter.Linq
{
    //What is this?
    //This is a cache of known generic methods that the translator uses
    //This way, we can have a zero cost cache of methods for each entity type
    //public static fields in a generic class are often a bug, but not in this case
    //we explicitly want to be able from one generic class get hold of the generic methods of the same type w/o lookups
    public static class KnownMethods<T>
    {
        private static readonly IQueryable<T> IQueryable = null;
        private static readonly Queryable<T> Queryable = null;

        public static readonly MethodInfo IQueryableTake = MethodFromExample(() => IQueryable.Take(0));

        public static readonly MethodInfo IQueryableOrderBy = MethodFromExample(() => IQueryable.OrderBy(i => i)).GetGenericMethodDefinition();
        public static readonly MethodInfo IQueryableOrderByDesc = MethodFromExample(() => IQueryable.OrderByDescending(i => i)).GetGenericMethodDefinition();
        public static readonly MethodInfo IQueryableThenBy = MethodFromExample(() => IQueryable.OrderBy(i => i).ThenBy(i => i)).GetGenericMethodDefinition();
        public static readonly MethodInfo IQueryableThenByDesc = MethodFromExample(() => IQueryable.OrderByDescending(i => i).ThenByDescending(i => i)).GetGenericMethodDefinition();

        public static readonly MethodInfo QueryableFirstOrDefault = MethodFromExample(() => Queryable.FirstOrDefault());
        public static readonly MethodInfo QueryableFirstOrDefaultPred = MethodFromExample(() => Queryable.FirstOrDefault(i => true));
        public static readonly MethodInfo IQueryableFirstOrDefault = MethodFromExample(() => IQueryable.FirstOrDefault());
        public static readonly MethodInfo IQueryableFirstOrDefaultPred = MethodFromExample(() => IQueryable.FirstOrDefault(i => true));
        public static readonly MethodInfo IQueryableWhere = MethodFromExample(() => IQueryable.Where(i => true),false);

        //This takes an expression lambda and extracts the contained method.
        //This way, we can by example specify exactly what overload we want, instead of looking up by name and args
        private static MethodInfo MethodFromExample<TIgnore>(Expression<Func<TIgnore>> fun,bool lift=true)
        {
            if (fun.Body is MethodCallExpression call)
            {
                var method = call.Method;
                //if (lift && method.IsGenericMethod)
                //    method = method.GetGenericMethodDefinition();
                return method;
            }
            throw new NotSupportedException();
        }
    }

    //These are all the non generic known methods
    public static class KnownMethods
    {
        private static readonly int[] Enumerable = null;

        public static readonly MethodInfo EnumerableContains = MethodFromExample(() => Enumerable.Contains(0));
        public static readonly MethodInfo StringContains = MethodFromExample(() => "".Contains(""));
        public static readonly MethodInfo StringStartsWith = MethodFromExample(() => "".StartsWith(""));
        public static readonly MethodInfo StringEndsWith = MethodFromExample(() => "".EndsWith(""));

        private static MethodInfo MethodFromExample<TIgnore>(Expression<Func<TIgnore>> fun)
        {
            if (fun.Body is MethodCallExpression call)
            {
                var method = call.Method;
                if (method.IsGenericMethod)
                    method = method.GetGenericMethodDefinition();
                return method;
            }
            throw new NotSupportedException();
        }
    }
}