using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Starcounter.Database;

// ReSharper disable StaticMemberInGenericType
// ReSharper disable InconsistentNaming

namespace Starcounter.Linq
{
    //What is this?
    //This is a cache of known generic methods that the translator uses
    //This way, we can have a zero cost cache of methods for each entity type
    //public static fields in a generic class are often a bug, but not in this case
    //we explicitly want to be able from one generic class get hold of the generic methods of the same type w/o lookups
    internal static class KnownMethods<TEntity>
    {
        private static readonly Queryable<TEntity> Queryable = null;
        private static readonly List<TEntity> EnumerableList = null;

        private static readonly Type collectionType = typeof(ICollection<>);

        private static readonly MethodInfo ListContains = MethodFromExample(() => EnumerableList.Contains(default(TEntity)));

        public static readonly MethodInfo QueryableDeletePred = MethodFromExample(() => Queryable.Delete(i => true));
        public static readonly MethodInfo QueryableDeleteAll = MethodFromExample(() => Queryable.DeleteAll());

        public static readonly MethodInfo QueryableFirstOrDefaultPred = MethodFromExample(() => Queryable.FirstOrDefault(i => true));
        public static readonly MethodInfo QueryableFirstOrDefault = MethodFromExample(() => Queryable.FirstOrDefault());

        public static readonly MethodInfo QueryableAnyPred = MethodFromExample(() => Queryable.Any(i => true));
        public static readonly MethodInfo QueryableAny = MethodFromExample(() => Queryable.Any());

        public static bool IsCollectionContains(MethodInfo method)
        {
            return method == ListContains ||
                   method.Name == nameof(ICollection<object>.Contains) &&
                   method.GetParameters().Length == 1 &&
                   method.DeclaringType != null &&
                   method.DeclaringType.GetGenericTypeDefinition().GetInterfaces().Any(x => x.GUID == collectionType.GUID);
        }

        //This takes an expression lambda and extracts the contained method.
        //This way, we can by example specify exactly what overload we want, instead of looking up by name and args
        private static MethodInfo MethodFromExample<TIgnore>(Expression<Func<TIgnore>> fun)
        {
            if (fun.Body is MethodCallExpression call)
            {
                return call.Method;
            }
            throw new NotSupportedException();
        }
        private static MethodInfo MethodFromExample(Expression<Action> fun)
        {
            if (fun.Body is MethodCallExpression call)
            {
                return call.Method;
            }
            throw new NotSupportedException();
        }
    }

    //These are all the non generic known methods
    internal static class KnownMethods
    {
        private static readonly IEnumerable<object> Enumerable = null;
        private static readonly IQueryable<object> Queryable = null;
        private static readonly object obj = new object();

        private static readonly IDatabaseContext database = null;

        public static readonly MethodInfo GetOid = MethodFromExample(() => database.GetOid(obj));
        public static readonly MethodInfo ObjectEquals = MethodFromExample(() => obj.Equals(null));
        public static readonly MethodInfo EnumerableContains = MethodFromExample(() => Enumerable.Contains(0));
        public static readonly MethodInfo StringContains = MethodFromExample(() => "".Contains(""));
        public static readonly MethodInfo StringStartsWith = MethodFromExample(() => "".StartsWith(""));
        public static readonly MethodInfo StringEndsWith = MethodFromExample(() => "".EndsWith(""));

        public static readonly MethodInfo EnumerableCount = MethodFromExample(() => Enumerable.Count());
        public static readonly MethodInfo EnumerableLongCount = MethodFromExample(() => Enumerable.LongCount());
        public static readonly MethodInfo EnumerableCountPredicate = MethodFromExample(() => Enumerable.Count(i => true));
        public static readonly MethodInfo EnumerableLongCountPredicate = MethodFromExample(() => Enumerable.LongCount(i => true));
        public static readonly MethodInfo QueryableCount = MethodFromExample(() => Queryable.Count());
        public static readonly MethodInfo QueryableLongCount = MethodFromExample(() => Queryable.LongCount());
        public static readonly MethodInfo QueryableCountPredicate = MethodFromExample(() => Queryable.Count(i => true));
        public static readonly MethodInfo QueryableLongCountPredicate = MethodFromExample(() => Queryable.LongCount(i => true));

        public static readonly MethodInfo EnumerableMaxObject = MethodFromExample(() => Enumerable.Max(i => i));
        public static readonly MethodInfo EnumerableMaxInt32 = MethodFromExample(() => Enumerable.Max(i => 1));
        public static readonly MethodInfo EnumerableMaxInt64 = MethodFromExample(() => Enumerable.Max(i => 1L));
        public static readonly MethodInfo EnumerableMaxSingle = MethodFromExample(() => Enumerable.Max(i => 1F));
        public static readonly MethodInfo EnumerableMaxDouble = MethodFromExample(() => Enumerable.Max(i => 1D));
        public static readonly MethodInfo EnumerableMaxDecimal = MethodFromExample(() => Enumerable.Max(i => 1M));
        public static readonly MethodInfo EnumerableMaxDirect = MethodFromExample(() => Enumerable.Max());
        public static readonly MethodInfo QueryableMax = MethodFromExample(() => Queryable.Max(i => i));

        public static bool IsEnumerableMax(MethodInfo method)
        {
            return method == EnumerableMaxObject || method == EnumerableMaxDecimal ||
                   method == EnumerableMaxInt32 || method == EnumerableMaxInt64 ||
                   method == EnumerableMaxSingle || method == EnumerableMaxDouble;
        }

        public static readonly MethodInfo EnumerableMinObject = MethodFromExample(() => Enumerable.Min(i => i));
        public static readonly MethodInfo EnumerableMinInt32 = MethodFromExample(() => Enumerable.Min(i => 1));
        public static readonly MethodInfo EnumerableMinInt64 = MethodFromExample(() => Enumerable.Min(i => 1L));
        public static readonly MethodInfo EnumerableMinSingle = MethodFromExample(() => Enumerable.Min(i => 1F));
        public static readonly MethodInfo EnumerableMinDouble = MethodFromExample(() => Enumerable.Min(i => 1D));
        public static readonly MethodInfo EnumerableMinDecimal = MethodFromExample(() => Enumerable.Min(i => 1M));
        public static readonly MethodInfo EnumerableMinDirect = MethodFromExample(() => Enumerable.Min());
        public static readonly MethodInfo QueryableMin = MethodFromExample(() => Queryable.Min(i => i));

        public static bool IsEnumerableMin(MethodInfo method)
        {
            return method == EnumerableMinObject || method == EnumerableMinDecimal ||
                   method == EnumerableMinInt32 || method == EnumerableMinInt64 ||
                   method == EnumerableMinSingle || method == EnumerableMinDouble;
        }

        private static readonly MethodInfo EnumerableAverageInt32 = MethodFromExample(() => Enumerable.Average(i => 1));
        private static readonly MethodInfo EnumerableAverageUInt32 = MethodFromExample(() => Enumerable.Average(i => 1U));
        private static readonly MethodInfo EnumerableAverageInt64 = MethodFromExample(() => Enumerable.Average(i => 1L));
        private static readonly MethodInfo EnumerableAverageDecimal = MethodFromExample(() => Enumerable.Average(i => 1M));
        private static readonly MethodInfo EnumerableAverageFloat = MethodFromExample(() => Enumerable.Average(i => 1F));
        private static readonly MethodInfo EnumerableAverageDouble = MethodFromExample(() => Enumerable.Average(i => 1D));
        private static readonly MethodInfo QueryableAverageInt32 = MethodFromExample(() => Queryable.Average(i => 1));
        private static readonly MethodInfo QueryableAverageUInt32 = MethodFromExample(() => Queryable.Average(i => 1U));
        private static readonly MethodInfo QueryableAverageInt64 = MethodFromExample(() => Queryable.Average(i => 1L));
        private static readonly MethodInfo QueryableAverageDecimal = MethodFromExample(() => Queryable.Average(i => 1M));
        private static readonly MethodInfo QueryableAverageFloat = MethodFromExample(() => Queryable.Average(i => 1F));
        private static readonly MethodInfo QueryableAverageDouble = MethodFromExample(() => Queryable.Average(i => 1D));

        public static bool IsEnumerableAverage(MethodInfo method)
        {
            return method == EnumerableAverageInt32 || method == EnumerableAverageInt64 ||
                   method == EnumerableAverageUInt32 || method == EnumerableAverageDecimal ||
                   method == EnumerableAverageFloat || method == EnumerableAverageDouble;
        }
        public static bool IsQueryableAverage(MethodInfo method)
        {
            return method == QueryableAverageInt32 || method == QueryableAverageInt64 ||
                   method == QueryableAverageUInt32 || method == QueryableAverageDecimal ||
                   method == QueryableAverageFloat || method == QueryableAverageDouble;
        }

        private static readonly MethodInfo EnumerableSumInt32 = MethodFromExample(() => Enumerable.Sum(i => 1));
        private static readonly MethodInfo EnumerableSumUInt32 = MethodFromExample(() => Enumerable.Sum(i => 1U));
        private static readonly MethodInfo EnumerableSumInt64 = MethodFromExample(() => Enumerable.Sum(i => 1L));
        private static readonly MethodInfo EnumerableSumDecimal = MethodFromExample(() => Enumerable.Sum(i => 1M));
        private static readonly MethodInfo EnumerableSumFloat = MethodFromExample(() => Enumerable.Sum(i => 1F));
        private static readonly MethodInfo EnumerableSumDouble = MethodFromExample(() => Enumerable.Sum(i => 1D));
        private static readonly MethodInfo QueryableSumInt32 = MethodFromExample(() => Queryable.Sum(i => 1));
        private static readonly MethodInfo QueryableSumUInt32 = MethodFromExample(() => Queryable.Sum(i => 1U));
        private static readonly MethodInfo QueryableSumInt64 = MethodFromExample(() => Queryable.Sum(i => 1L));
        private static readonly MethodInfo QueryableSumDecimal = MethodFromExample(() => Queryable.Sum(i => 1M));
        private static readonly MethodInfo QueryableSumFloat = MethodFromExample(() => Queryable.Sum(i => 1F));
        private static readonly MethodInfo QueryableSumDouble = MethodFromExample(() => Queryable.Sum(i => 1D));

        public static bool IsEnumerableSum(MethodInfo method)
        {
            return method == EnumerableSumInt32 || method == EnumerableSumInt64 ||
                   method == EnumerableSumUInt32 || method == EnumerableSumDecimal ||
                   method == EnumerableSumFloat || method == EnumerableSumDouble;
        }
        public static bool IsQueryableSum(MethodInfo method)
        {
            return method == QueryableSumInt32 || method == QueryableSumInt64 ||
                   method == QueryableSumUInt32 || method == QueryableSumDecimal ||
                   method == QueryableSumFloat || method == QueryableSumDouble;
        }


        //OrderBy et al have a generic arity of two, <TEntity,TValue> this means we cannot use the generic cache to store
        //references to the generic method instances, we simply don't know the TValue at compiletime/startup
        public static readonly MethodInfo QueryableOrderBy = MethodFromExample(() => Queryable.OrderBy(i => i));
        public static readonly MethodInfo QueryableOrderByDesc = MethodFromExample(() => Queryable.OrderByDescending(i => i));
        public static readonly MethodInfo QueryableThenBy = MethodFromExample(() => Queryable.OrderBy(i => i).ThenBy(i => i));
        public static readonly MethodInfo QueryableThenByDesc = MethodFromExample(() => Queryable.OrderByDescending(i => i).ThenByDescending(i => i));

        public static readonly MethodInfo QueryableSelect = MethodFromExample(() => Queryable.Select(i => i));
        public static readonly MethodInfo QueryableWhere = MethodFromExample(() => Queryable.Where(i => true));
        public static readonly MethodInfo QueryableGroupBy = MethodFromExample(() => Queryable.GroupBy(i => i));
        public static readonly MethodInfo QueryableTake = MethodFromExample(() => Queryable.Take(0));
        public static readonly MethodInfo QueryableSkip = MethodFromExample(() => Queryable.Skip(0));

        public static readonly MethodInfo QueryableFirstOrDefaultPred = MethodFromExample(() => Queryable.FirstOrDefault(i => true));
        public static readonly MethodInfo QueryableFirstOrDefault = MethodFromExample(() => Queryable.FirstOrDefault());
        public static readonly MethodInfo QueryableFirstPred = MethodFromExample(() => Queryable.First(i => true));
        public static readonly MethodInfo QueryableFirst = MethodFromExample(() => Queryable.First());
        public static readonly MethodInfo QueryableSingleOrDefaultPred = MethodFromExample(() => Queryable.SingleOrDefault(i => true));
        public static readonly MethodInfo QueryableSingleOrDefault = MethodFromExample(() => Queryable.SingleOrDefault());
        public static readonly MethodInfo QueryableSinglePred = MethodFromExample(() => Queryable.Single(i => true));
        public static readonly MethodInfo QueryableSingle = MethodFromExample(() => Queryable.Single());

        public static readonly MethodInfo QueryableAllPred = MethodFromExample(() => Queryable.All(i => true));
        public static readonly MethodInfo QueryableAnyPred = MethodFromExample(() => Queryable.Any(i => true));
        public static readonly MethodInfo QueryableAny = MethodFromExample(() => Queryable.Any());

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
