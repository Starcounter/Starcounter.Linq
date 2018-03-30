using System;

namespace Starcounter.Linq.QueryTests.Utilities
{
    public sealed class TestAppHost : TempAppHost
    {
        public static TestAppHost Current = new TestAppHost();
        
        private TestAppHost() : base(0)
        {
            // this should be called only once for all tests.
            Start();
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
