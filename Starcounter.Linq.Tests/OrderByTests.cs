using System.Linq;
using Xunit;
using static Starcounter.Linq.Tests.Utils;
using static Starcounter.Linq.DbLinq;
namespace Starcounter.Linq.Tests
{
    public class OrderByTests
    {
        [Fact]
        public void OrderBy()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P ORDER BY P.Age ASC",
                Sql(() => Objects<Person>().OrderBy(p => p.Age)));
        }

        [Fact]
        public void OrderByMultiple()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P ORDER BY P.Age ASC, P.Name DESC",
                Sql(() => Objects<Person>().OrderBy(p => p.Age).ThenByDescending(p => p.Name)));
        }
    }
}
