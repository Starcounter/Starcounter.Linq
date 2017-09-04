using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;

namespace Starcounter.Linq.Tests
{
    public class TakeTests
    {
        [Fact]
        public void Take()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P FETCH ?",
                Sql(() => Objects<Person>().Take(10)));
        }

        [Fact]
        public void Skip()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P FETCH ? OFFSET ?",
                Sql(() => Objects<Person>().Skip(20).Take(10)));
        }
    }
}
