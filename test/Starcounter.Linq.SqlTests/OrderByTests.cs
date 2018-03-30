using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class OrderByTests
    {
        [Fact]
        public void OrderBy()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P ORDER BY P.\"Age\" ASC",
                Sql(() => Objects<Person>().OrderBy(p => p.Age)));
        }

        [Fact]
        public void OrderByMultiple()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P ORDER BY P.\"Age\" ASC, P.\"Name\" DESC",
                Sql(() => Objects<Person>().OrderBy(p => p.Age).ThenByDescending(p => p.Name)));
        }

        [Fact]
        public void OrderBy_ReservedWordField()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P ORDER BY P.\"Limit\" ASC",
                Sql(() => Objects<Person>().OrderBy(p => p.Limit)));
        }
    }
}
