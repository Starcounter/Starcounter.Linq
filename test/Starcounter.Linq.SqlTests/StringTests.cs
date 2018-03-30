using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class StringTests
    {
        [Fact]
        public void StringContains()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" LIKE '%' || ? || '%'))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name.Contains("XXX"))));
        }

        [Fact]
        public void StringContains_CalculatedValue()
        {
            var name = "XXX";
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" LIKE '%' || ? || '%'))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name.Contains(name))));
        }

        [Fact]
        public void StringStartsWith()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" LIKE ? || '%'))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name.StartsWith("XXX"))));
        }

        [Fact]
        public void StringEndsWith()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" LIKE '%' || ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name.EndsWith("XXX"))));
        }
    }
}