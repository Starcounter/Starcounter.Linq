using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;

namespace Starcounter.Linq.Tests
{
    public class BooleanTests
    {
        [Fact]
        public void BooleanTrue()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Disabled\" = True)) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Disabled)));
        }

        [Fact]
        public void BooleanFalse()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (NOT (E.\"Disabled\" = True)) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => !p.Disabled)));
        }

        [Fact]
        public void BooleanEquals()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Disabled\" = ?)) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Disabled == true)));
        }

        [Fact]
        public void BooleanEqualsNot()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Disabled\" <> ?)) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Disabled != true)));
        }

        [Fact]
        public void BooleanTrueComplex()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" = True))) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Age > 0 && p.Disabled)));
        }

        [Fact]
        public void BooleanFalseComplex()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND NOT (E.\"Disabled\" = True))) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Age > 0 && !p.Disabled)));
        }

        [Fact]
        public void BooleanEqualsComplex()
        {
            var fl = false;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" = ?))) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Age > 0 && p.Disabled == fl)));
        }

        [Fact]
        public void BooleanEqualsNotComplex()
        {
            var fl = false;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" <> ?))) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Age > 0 && p.Disabled != fl)));
        }
    }
}
