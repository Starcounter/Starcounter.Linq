using System;
using Starcounter.Linq.QueryTests.Utilities;

namespace Starcounter.Linq.QueryTests
{
    public class DataTestFixture : IDisposable
    {
        internal TestAppHost AppHost;

        public DataTestFixture()
        {
            AppHost = TestAppHost.GetInstance();
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        public void RecreateData()
        {
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        public void Dispose()
        {
            DataHelper.ResetData();
            AppHost.Dispose();
        }
    }
}