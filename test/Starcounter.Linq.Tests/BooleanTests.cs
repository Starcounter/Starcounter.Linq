using System.Linq;
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
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Disabled\" = True))",
                Sql(() => Objects<Employee>().Where(p => p.Disabled)));
        }

        [Fact]
        public void BooleanFalse()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (NOT (E.\"Disabled\" = True))",
                Sql(() => Objects<Employee>().Where(p => !p.Disabled)));
        }

        [Fact]
        public void BooleanEquals()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Disabled\" = ?))",
                Sql(() => Objects<Employee>().Where(p => p.Disabled == true)));
        }

        [Fact]
        public void BooleanEqualsNot()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Disabled\" <> ?))",
                Sql(() => Objects<Employee>().Where(p => p.Disabled != true)));
        }

        [Fact]
        public void BooleanTrueComplex()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" = True)))",
                Sql(() => Objects<Employee>().Where(p => p.Age > 0 && p.Disabled)));
        }

        [Fact]
        public void BooleanFalseComplex()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND NOT (E.\"Disabled\" = True)))",
                Sql(() => Objects<Employee>().Where(p => p.Age > 0 && !p.Disabled)));
        }

        [Fact]
        public void BooleanEqualsComplex()
        {
            var fl = false;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" = ?)))",
                Sql(() => Objects<Employee>().Where(p => p.Age > 0 && p.Disabled == fl)));
        }

        [Fact]
        public void BooleanEqualsNotComplex()
        {
            var fl = false;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" <> ?)))",
                Sql(() => Objects<Employee>().Where(p => p.Age > 0 && p.Disabled != fl)));
        }
    }
}
