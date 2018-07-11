using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class PagingTests
    {
        [Fact]
        public void Take()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P FETCH 10",
                Sql(() => Objects<Person>().Take(10)));
        }

        [Fact]
        public void Take_CalculatedValue()
        {
            var takeValue = 10;
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P FETCH 10",
                Sql(() => Objects<Person>().Take(takeValue)));
        }

        [Fact]
        public void Skip()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P FETCH 10 OFFSET 20",
                Sql(() => Objects<Person>().Skip(20).Take(10)));
        }

        [Fact]
        public void Skip_CalculatedValue()
        {
            var skipValue = 20;
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P FETCH 10 OFFSET 20",
                Sql(() => Objects<Person>().Skip(skipValue).Take(10)));
        }

        [Fact]
        public void FirstOrDefault()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P",
                Sql(() => Objects<Person>().FirstOrDefault()));
        }

        [Fact]
        public void First()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P",
                Sql(() => Objects<Person>().First()));
        }

        [Fact]
        public void FirstOrDefault_Predicate()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name == "XXX")));
        }

        [Fact]
        public void First_Predicate()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                Sql(() => Objects<Person>().First(p => p.Name == "XXX")));
        }
    }
}
