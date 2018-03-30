using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class BooleanTests
    {
        [Fact]
        public void BooleanTrue()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Disabled\" = True))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Disabled)));
        }

        [Fact]
        public void BooleanFalse()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (NOT (E.\"Disabled\" = True))",
                (string)Sql(() => Objects<Employee>().Where(p => !p.Disabled)));
        }

        [Fact]
        public void BooleanTrue_Nested()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Global\" = True))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Global)));
        }

        [Fact]
        public void BooleanTrue_NestedTwice()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\".\"Global\" = True))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Company.Global)));
        }

        [Fact]
        public void BooleanFalse_NestedTwice()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (NOT (E.\"Department\".\"Company\".\"Global\" = True))",
                (string)Sql(() => Objects<Employee>().Where(p => !p.Department.Company.Global)));
        }

        [Fact]
        public void BooleanEquals()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Disabled\" = ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Disabled == true)));
        }

        [Fact]
        public void BooleanEqualsNot()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Disabled\" <> ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Disabled != false)));
        }

        [Fact]
        public void BooleanEquals_Variable_Nested()
        {
            var fl = false;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Global\" = ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Global == fl)));
        }

        [Fact]
        public void BooleanEqualsNot_Variable_Nested()
        {
            var fl = false;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Global\" <> ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Global != fl)));
        }

        [Fact]
        public void BooleanEquals_NestedTwice()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\".\"Global\" = ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Company.Global == false)));
        }

        [Fact]
        public void BooleanEqualsNot_NestedTwice()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\".\"Global\" <> ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Company.Global != false)));
        }

        [Fact]
        public void BooleanTrueComplex()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" = True)))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Age > 0 && p.Disabled)));
        }

        [Fact]
        public void BooleanFalseComplex()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND NOT (E.\"Disabled\" = True)))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Age > 0 && !p.Disabled)));
        }

        [Fact]
        public void BooleanEqualsComplex()
        {
            var fl = false;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" = ?)))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Age > 0 && p.Disabled == fl)));
        }

        [Fact]
        public void BooleanEqualsNotComplex()
        {
            var fl = false;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (((E.\"Age\" > ?) AND (E.\"Disabled\" <> ?)))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Age > 0 && p.Disabled != fl)));
        }
    }
}
