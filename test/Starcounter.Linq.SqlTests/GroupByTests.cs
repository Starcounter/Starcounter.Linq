﻿using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class GroupByTests
    {
        [Fact]
        public void GroupBy_Count()
        {
            Assert.Equal("SELECT COUNT(P) FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.Count())));
        }

        [Fact]
        public void GroupBy_LongCount()
        {
            Assert.Equal("SELECT COUNT(P) FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.LongCount())));
        }

        [Fact]
        public void GroupBy_CountPred()
        {
            Assert.Equal("SELECT COUNT(P) FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Limit\" > ? GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.Count(p => p.Limit > 0))));
        }

        [Fact]
        public void GroupBy_LongCountPred()
        {
            Assert.Equal("SELECT COUNT(P) FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE P.\"Limit\" > ? GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.LongCount(p => p.Limit > 0))));
        }

        [Fact]
        public void GroupBy_Max()
        {
            Assert.Equal("SELECT MAX(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.Max(p => p.Age))));
        }

        [Fact]
        public void GroupBy_Min()
        {
            Assert.Equal("SELECT MIN(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.Min(p => p.Age))));
        }

        [Fact]
        public void GroupBy_Sum()
        {
            Assert.Equal("SELECT SUM(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.Sum(p => p.Age))));
        }

        [Fact]
        public void GroupBy_Average()
        {
            Assert.Equal("SELECT AVG(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P GROUP BY P.\"Gender\"",
                Sql(() => Objects<Person>().GroupBy(p => p.Gender).Select(x => x.Average(p => p.Age))));
        }
    }
}
