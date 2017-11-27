using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;

namespace Starcounter.Linq.Tests
{
    public class LinqTests
    {
        [Fact]
        public void NestedValueEquals_Complex()
        {
            var age = 123;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE (((E.\"Department\".\"Company\".\"Name\" = ?) AND (E.\"Age\" > ?))) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "XXX" && p.Age > age)));
        }

        [Fact]
        public void ValueEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name == "XXX")));
        }

        [Fact]
        public void ValueEquals_ReservedWordField()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Limit\" = ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Limit == 5)));
        }

        [Fact]
        public void NestedValueEquals_Complex_ReservedWordField()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\".\"Index\" = ?)) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(p => p.Department.Company.Index == 0)));
        }

        [Fact]
        public void ValueNotEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" <> ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name != "XXX")));
        }

        [Fact]
        public void ValueEqualsNot()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (NOT (P.\"Name\" = ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => !(p.Name == "XXX"))));
        }

        [Fact]
        public void ValueGreaterThan()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Age\" > ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age > 123)));
        }

        [Fact]
        public void ValueGreaterOrEqualTo()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Age\" >= ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age >= 123)));
        }

        [Fact]
        public void ValueLessThan()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Age\" < ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age < 123)));
        }

        [Fact]
        public void ValueLessOrEqualTo()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Age\" <= ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age <= 123)));
        }

        [Fact]
        public void StringContains()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" LIKE '%' || ? || '%')) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.Contains("XXX"))));
        }

        [Fact]
        public void StringContains_CalculatedValue()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" LIKE '%' || ? || '%')) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.Contains(name))));
        }

        [Fact]
        public void StringStartsWith()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" LIKE ? || '%')) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("XXX"))));
        }

        [Fact]
        public void StringEndsWith()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" LIKE '%' || ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("XXX"))));
        }

        [Fact]
        public void TypeIs()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P IS \"Starcounter\".\"Linq\".\"Tests\".\"Employee\")) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p is Employee)));
        }

#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type
        // ReSharper disable IsExpressionAlwaysTrue
        [Fact]
        public void TypeIs_Nested()
        {
            Assert.Equal("SELECT D FROM \"Starcounter\".\"Linq\".\"Tests\".\"Department\" D WHERE ((D.\"Company\" IS \"Starcounter\".\"Linq\".\"Tests\".\"Company\")) FETCH 1",
                Sql(() => Objects<Department>().FirstOrDefault(d => d.Company is Company)));
        }

        [Fact]
        public void TypeIs_NestedTwice()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\" IS \"Starcounter\".\"Linq\".\"Tests\".\"Company\")) FETCH 1",
                Sql(() => Objects<Employee>().FirstOrDefault(d => d.Department.Company is Company)));
        }
        // ReSharper restore IsExpressionAlwaysTrue

        // ReSharper disable IsExpressionAlwaysFalse
        [Fact]
        public void TypeIs_ReservedWordType()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P IS \"Starcounter\".\"Linq\".\"Tests\".\"Unknown\")) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p is Unknown)));
        }
        // ReSharper restore IsExpressionAlwaysFalse
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type

        [Fact]
        public void TypeIsNot()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (NOT (P IS \"Starcounter\".\"Linq\".\"Tests\".\"Employee\")) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => !(p is Employee))));
        }

        [Fact]
        public void EnumerableContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Age\" = ?) OR (P.\"Age\" = ?) OR (P.\"Age\" = ?))) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age))));
        }

        [Fact]
        public void EnumerableNotContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (NOT ((P.\"Age\" = ?) OR (P.\"Age\" = ?) OR (P.\"Age\" = ?))) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => !ages.Contains(p.Age))));
        }
    }
}
