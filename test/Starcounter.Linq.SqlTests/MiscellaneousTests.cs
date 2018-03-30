using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class MiscellaneousTests
    {
        [Fact]
        public void WhereEquals_Multiple()
        {
            var name = "XXX";
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" = ?)) AND ((P.\"Age\" = ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name == name).Where(p => p.Age == 1)));
        }

        [Fact]
        public void WhereEquals_Or()
        {
            var name = "XXX";
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (((P.\"Name\" = ?) OR (P.\"Age\" = ?)))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name == name || p.Age == 1)));
        }

        [Fact]
        public void WhereEquals_And()
        {
            var name = "XXX";
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (((P.\"Name\" = ?) AND (P.\"Age\" = ?)))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name == name && p.Age == 1)));
        }

        [Fact]
        public void NestedValue_Equals_And()
        {
            var age = 123;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (((E.\"Department\".\"Company\".\"Name\" = ?) AND (E.\"Age\" > ?)))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Company.Name == "XXX" && p.Age > age)));
        }

        [Fact]
        public void ReservedWordField_ValueEquals()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Limit\" = ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Limit == 5)));
        }

        [Fact]
        public void ReservedWordField_NestedValueEquals()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\".\"Index\" = ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Company.Index == 0)));
        }

        [Fact]
        public void EnumerableContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (((P.\"Age\" = ?) OR (P.\"Age\" = ?) OR (P.\"Age\" = ?)))",
                (string)Sql(() => Objects<Person>().Where(p => ages.Contains(p.Age))));
        }

        [Fact]
        public void EnumerableNotContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (NOT ((P.\"Age\" = ?) OR (P.\"Age\" = ?) OR (P.\"Age\" = ?)))",
                (string)Sql(() => Objects<Person>().Where(p => !ages.Contains(p.Age))));
        }
    }
}
