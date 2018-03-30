using System;
using Starcounter.Linq.QueryTests.Utilities;

namespace Starcounter.Linq.QueryTests
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