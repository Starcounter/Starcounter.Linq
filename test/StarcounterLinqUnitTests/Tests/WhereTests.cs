using System;
using System.Linq;
using Starcounter;
using Starcounter.Linq;
using StarcounterLinqUnitTests.Helpers;
using Xunit;

namespace StarcounterLinqUnitTests.Tests
{
    public class WhereTestsFixture : IDisposable
    {
        public WhereTestsFixture()
        {
            Scheduling.ScheduleTask(() =>
            {
                DataHelper.ResetData();
                DataHelper.CreateEmployees();
            }, waitForCompletion: true);
        }

        public void Dispose()
        {
            Scheduling.ScheduleTask(DataHelper.ResetData, waitForCompletion: true);
        }
    }

    public class WhereTests : IClassFixture<WhereTestsFixture>
    {
        public WhereTests(WhereTestsFixture fixture)
        {
        }

        [Fact]
        public void SelectPersonsByName()
        {
            Scheduling.ScheduleTask(() =>
            {
                var persons = DbLinq.Objects<Person>().Where(p => p.Name == "Roger").ToList();
                Assert.Equal(1, persons.Count);
            }, waitForCompletion: true);
        }
    }
}