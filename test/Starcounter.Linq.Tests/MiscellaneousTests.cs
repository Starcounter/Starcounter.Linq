using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;

namespace Starcounter.Linq.Tests
{
    public class MiscellaneousTests
    {
        [Fact]
        public void WhereEquals_Multiple()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?)) AND ((P.\"Age\" = ?))",
                Sql(() => Objects<Person>().Where(p => p.Name == name).Where(p => p.Age == 1)));
        }

        [Fact]
        public void WhereEquals_Or()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Name\" = ?) OR (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => p.Name == name || p.Age == 1)));
        }

        [Fact]
        public void WhereEquals_And()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Name\" = ?) AND (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => p.Name == name && p.Age == 1)));
        }

        [Fact]
        public void NestedValue_Equals_And()
        {
            var age = 123;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Department\".\"Company\".\"Name\" = ?) AND (E.\"Age\" > ?)))",
                Sql(() => Objects<Employee>().Where(p => p.Department.Company.Name == "XXX" && p.Age > age)));
        }

        [Fact]
        public void ReservedWordField_ValueEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Limit\" = ?))",
                Sql(() => Objects<Person>().Where(p => p.Limit == 5)));
        }

        [Fact]
        public void ReservedWordField_NestedValueEquals()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\".\"Index\" = ?))",
                Sql(() => Objects<Employee>().Where(p => p.Department.Company.Index == 0)));
        }

        [Fact]
        public void EnumerableContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Age\" = ?) OR (P.\"Age\" = ?) OR (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => ages.Contains(p.Age))));
        }

        [Fact]
        public void EnumerableNotContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (NOT ((P.\"Age\" = ?) OR (P.\"Age\" = ?) OR (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => !ages.Contains(p.Age))));
        }

        [Fact]
        public void Delete()
        {
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" WHERE ((\"Age\" > ?))",
                Sql<Person>(() => Objects<Person>().Delete(x => x.Age > 0)));
        }

        [Fact]
        public void DeleteAll()
        {
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\"",
                Sql<Person>(() => Objects<Person>().DeleteAll()));
        }
    }
}
