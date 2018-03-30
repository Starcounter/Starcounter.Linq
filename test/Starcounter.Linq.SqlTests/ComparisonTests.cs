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
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name == "XXX")));
        }

        [Fact]
        public void ValueIsNull()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NULL))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department == null)));
        }

        [Fact]
        public void ValueIsNotNull()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NOT NULL))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department != null)));
        }

        [Fact]
        public void ValueIsNullVariable()
        {
            Department department = null;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NULL))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department == department)));
        }

        [Fact]
        public void ValueIsNotNullVariable()
        {
            Department department = null;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NOT NULL))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department != department)));
        }

        [Fact]
        public void ObjectEqualsMethod_NullVariable()
        {
            Department department = null;
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" IS NULL))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Equals(department))));
        }

        [Fact]
        public void ObjectNotEqualsMethod_NullVariable()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (NOT (E.\"Department\" IS NULL))",
                (string)Sql(() => Objects<Employee>().Where(p => !p.Department.Equals(null))));
        }

        [Fact]
        public void ObjectEqualsMethod_NotNullVariable()
        {
            Department department = new Department();
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\" = ?))",
                (string)Sql(() => Objects<Employee>().Where(p => p.Department.Equals(department))));
        }

        [Fact]
        public void ObjectNotEqualsMethod_NotNullVariable()
        {
            Department department = new Department();
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE (NOT (E.\"Department\" = ?))",
                (string)Sql(() => Objects<Employee>().Where(p => !p.Department.Equals(department))));
        }

        [Fact]
        public void ValueNotEquals()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Name\" <> ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Name != "XXX")));
        }

        [Fact]
        public void ValueEqualsNot()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (NOT (P.\"Name\" = ?))",
                (string)Sql(() => Objects<Person>().Where(p => !(p.Name == "XXX"))));
        }

        [Fact]
        public void ValueGreaterThan()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" > ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Age > 123)));
        }

        [Fact]
        public void ValueGreaterOrEqualTo()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" >= ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Age >= 123)));
        }

        [Fact]
        public void ValueLessThan()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" < ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Age < 123)));
        }

        [Fact]
        public void ValueLessOrEqualTo()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P.\"Age\" <= ?))",
                (string)Sql(() => Objects<Person>().Where(p => p.Age <= 123)));
        }
    }
}