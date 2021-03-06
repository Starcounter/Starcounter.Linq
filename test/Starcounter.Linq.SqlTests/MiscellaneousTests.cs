﻿using System.Linq;
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
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (P.\"Name\" = ?) AND (P.\"Age\" = ?)",
                Sql(() => Objects<Person>().Where(p => p.Name == name).Where(p => p.Age == 1)));
        }

        [Fact]
        public void WhereEquals_Multiple2()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (P.\"Name\" = ?) AND (P.\"Age\" = ? OR P.\"Gender\" = ?)",
                Sql(() => Objects<Person>().Where(p => p.Name == name).Where(p => p.Age == 1 || p.Gender == Gender.Male)));
        }

        [Fact]
        public void WhereEquals_Multiple3()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (P.\"Age\" = ? OR P.\"Gender\" = ?) AND (P.\"Name\" = ?)",
                Sql(() => Objects<Person>().Where(p => p.Age == 1 || p.Gender == Gender.Male).Where(p => p.Name == name)));
        }

        [Fact]
        public void WhereEquals_Or()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Name\" = ? OR P.\"Age\" = ?",
                Sql(() => Objects<Person>().Where(p => p.Name == name || p.Age == 1)));
        }

        [Fact]
        public void WhereAndOr()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Name\" = ? AND P.\"Gender\" = ? OR P.\"Age\" = ?",
                Sql(() => Objects<Person>().Where(p => p.Name == name && p.Gender == Gender.Male || p.Age == 1)));
        }

        [Fact]
        public void WhereOrAnd()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Age\" = ? OR P.\"Name\" = ? AND P.\"Gender\" = ?",
                Sql(() => Objects<Person>().Where(p => p.Age == 1 || p.Name == name && p.Gender == Gender.Male)));
        }

        [Fact]
        public void WhereAndParenthesesOr()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Name\" = ? AND (P.\"Gender\" = ? OR P.\"Age\" = ?)",
                Sql(() => Objects<Person>().Where(p => p.Name == name && (p.Gender == Gender.Male || p.Age == 1))));
        }

        [Fact]
        public void WhereParenthesesOrAnd()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (P.\"Gender\" = ? OR P.\"Age\" = ?) AND P.\"Name\" = ?",
                Sql(() => Objects<Person>().Where(p => (p.Gender == Gender.Male || p.Age == 1) && p.Name == name)));
        }

        [Fact]
        public void WhereParenthesesOrAndParenthesesOr()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (P.\"Gender\" = ? OR P.\"Gender\" = ?) AND (P.\"Name\" = ? OR P.\"Age\" = ?)",
                Sql(() => Objects<Person>().Where(p => (p.Gender == Gender.Male || p.Gender == Gender.Female) && (p.Name == name || p.Age == 1))));
        }

        [Fact]
        public void WhereOrAndOr()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Gender\" = ? OR P.\"Gender\" = ? AND P.\"Name\" = ? OR P.\"Age\" = ?",
                Sql(() => Objects<Person>().Where(p => p.Gender == Gender.Male || p.Gender == Gender.Female && p.Name == name || p.Age == 1)));
        }

        [Fact]
        public void WhereEquals_And()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Name\" = ? AND P.\"Age\" = ?",
                Sql(() => Objects<Person>().Where(p => p.Name == name && p.Age == 1)));
        }

        [Fact]
        public void NestedValue_Equals_And()
        {
            var age = 123;
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE E.\"Department\".\"Company\".\"Name\" = ? AND E.\"Age\" > ?",
                Sql(() => Objects<Employee>().Where(p => p.Department.Company.Name == "XXX" && p.Age > age)));
        }

        [Fact]
        public void ReservedWordField_ValueEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Limit\" = ?",
                Sql(() => Objects<Person>().Where(p => p.Limit == 5)));
        }

        [Fact]
        public void ReservedWordField_NestedValueEquals()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE E.\"Department\".\"Company\".\"Index\" = ?",
                Sql(() => Objects<Employee>().Where(p => p.Department.Company.Index == 0)));
        }

        [Fact]
        public void EnumerableContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (P.\"Age\" = ? OR P.\"Age\" = ? OR P.\"Age\" = ?)",
                Sql(() => Objects<Person>().Where(p => ages.Contains(p.Age))));
        }

        [Fact]
        public void EnumerableNotContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE NOT (P.\"Age\" = ? OR P.\"Age\" = ? OR P.\"Age\" = ?)",
                Sql(() => Objects<Person>().Where(p => !ages.Contains(p.Age))));
        }

        [Fact]
        public void EnumerableEmptyContains()
        {
            var ages = new int[0];
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Age\" <> P.\"Age\"",
                Sql(() => Objects<Person>().Where(p => ages.Contains(p.Age))));
        }

        [Fact]
        public void EnumerableObjectsEmptyContains()
        {
            var people = new Person[0];
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P <> P",
                Sql(() => Objects<Person>().Where(p => people.Contains(p))));
        }

        [Fact]
        public void PredicateParameterComparison()
        {
            var dept = new Department();
            Assert.Equal("SELECT D FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Department\" D WHERE D = ?",
                Sql(() => Objects<Department>().Where(d => d == dept)));
        }
    }
}
