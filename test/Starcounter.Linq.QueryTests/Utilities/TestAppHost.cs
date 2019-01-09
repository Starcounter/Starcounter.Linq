using System;

namespace Starcounter.Linq.QueryTests.Utilities
{
    public sealed class TestAppHost : TempAppHost
    {
        private static TestAppHost instance;

        private TestAppHost() : base(0)
        {
            // this should be called only once for all tests.
            Start();
        }

        public static TestAppHost GetInstance()
        {
            return instance ?? (instance = new TestAppHost());
        }

        ~TestAppHost()
        {
            Dispose();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
