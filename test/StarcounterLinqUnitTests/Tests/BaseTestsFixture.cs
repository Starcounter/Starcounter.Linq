using System;
using StarcounterLinqUnitTests.Helpers;
using StarcounterLinqUnitTests.Utilities;

namespace StarcounterLinqUnitTests.Tests
{
    public class BaseTestsFixture : TestAppHost, IDisposable
    {
        public BaseTestsFixture() : base()
        {
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        public void RecreateData()
        {
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        public override void Dispose()
        {
            DataHelper.ResetData();
            base.Dispose();
        }
    }
}