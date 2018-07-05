using System;

namespace Starcounter.Linq.QueryTests
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
            Scheduling.RunTask(() => DataHelper.ResetData()).Wait();
        }
    }
}