using System;
using Starcounter;
using StarcounterLinqUnitTests.Helpers;

namespace StarcounterLinqUnitTests.Tests
{
    public class BaseTestsFixture : IDisposable
    {
        public BaseTestsFixture()
        {
            Scheduling.RunTask(() =>
            {
                DataHelper.ResetData();
                DataHelper.CreateEmployees();
            }).Wait();
        }

        public void RecreateData()
        {
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        public void Dispose()
        {
            Scheduling.RunTask(DataHelper.ResetData).Wait();
        }
    }
}