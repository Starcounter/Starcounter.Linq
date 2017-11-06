using System;
using Starcounter;
using StarcounterLinqUnitTests.Helpers;

namespace StarcounterLinqUnitTests.Tests
{
    public class BaseTestsFixture : IDisposable
    {
        public BaseTestsFixture()
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
}