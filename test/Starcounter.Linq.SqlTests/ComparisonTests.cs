using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class ComparisonTests
    {
        [Fact]
        public void ValueEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                Sql(() => Objects<Person>().Where(p => p.Name == "XXX")));
        }

        [Fact]
        public void ValueIsNull()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NULL))",
                Sql(() => Objects<Employee>().Where(p => p.Department == null)));
        }

        [Fact]
        public void ValueIsNotNull()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NOT NULL))",
                Sql(() => Objects<Employee>().Where(p => p.Department != null)));
        }

        [Fact]
        public void ValueIsNullVariable()
        {
            Department department = null;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NULL))",
                Sql(() => Objects<Employee>().Where(p => p.Department == department)));
        }

        [Fact]
        public void ValueIsNotNullVariable()
        {
            Department department = null;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NOT NULL))",
                Sql(() => Objects<Employee>().Where(p => p.Department != department)));
        }

        [Fact]
        public void ObjectEqualsMethod_NullVariable()
        {
            Department department = null;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NULL))",
                Sql(() => Objects<Employee>().Where(p => p.Department.Equals(department))));
        }

        [Fact]
        public void ObjectNotEqualsMethod_NullVariable()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (NOT (E.\"Department\" IS NULL))",
                Sql(() => Objects<Employee>().Where(p => !p.Department.Equals(null))));
        }

        [Fact]
        public void ObjectEqualsMethod_NotNullVariable()
        {
            Department department = new Department();
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" = ?))",
                Sql(() => Objects<Employee>().Where(p => p.Department.Equals(department))));
        }

        [Fact]
        public void ObjectNotEqualsMethod_NotNullVariable()
        {
            Department department = new Department();
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (NOT (E.\"Department\" = ?))",
                Sql(() => Objects<Employee>().Where(p => !p.Department.Equals(department))));
        }

        [Fact]
        public void ValueNotEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" <> ?))",
                Sql(() => Objects<Person>().Where(p => p.Name != "XXX")));
        }

        [Fact]
        public void ValueEqualsNot()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (NOT (P.\"Name\" = ?))",
                Sql(() => Objects<Person>().Where(p => !(p.Name == "XXX"))));
        }

        [Fact]
        public void ValueGreaterThan()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" > ?))",
                Sql(() => Objects<Person>().Where(p => p.Age > 123)));
        }

        [Fact]
        public void ValueGreaterOrEqualTo()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" >= ?))",
                Sql(() => Objects<Person>().Where(p => p.Age >= 123)));
        }

        [Fact]
        public void ValueLessThan()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" < ?))",
                Sql(() => Objects<Person>().Where(p => p.Age < 123)));
        }

        [Fact]
        public void ValueLessOrEqualTo()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" <= ?))",
                Sql(() => Objects<Person>().Where(p => p.Age <= 123)));
        }
    }
}
