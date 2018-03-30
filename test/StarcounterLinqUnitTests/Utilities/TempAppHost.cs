using System;
using System.Diagnostics;
using System.Threading;
using Starcounter.Nova.Abstractions;
using Starcounter.Nova.Hosting;

namespace StarcounterLinqUnitTests.Utilities
{
    /// <summary>
    /// Fixture that establish the host for a group of tests.
    /// </summary>
    /// <remarks>
    /// Read more about xUnit fixtures, and class fixtures in
    /// particular, here:
    /// https://xunit.github.io/docs/shared-context.html#class-fixture 
    /// </remarks>
    public class TempAppHost : TestFixtureBase, IDisposable
    {
        private TempDatabase _tmpDb;
        private IAppHost _apphost;
        private Stopwatch _createTempDatabaseSw = new Stopwatch();
        private Stopwatch _startingDatabaseSw = new Stopwatch();

        public TempAppHost(uint schedulerCount)
        {
            Console.WriteLine($"Environment variable StarcounterBin=\"{System.Environment.GetEnvironmentVariable("StarcounterBin") ?? ""}\"");
            Console.WriteLine($"Environment variable STAR_QP=\"{System.Environment.GetEnvironmentVariable("STAR_QP") ?? ""}\"");

            Console.Write("Creating temporary database ");
            _createTempDatabaseSw.Start();
            _tmpDb = new TempDatabase();
            _createTempDatabaseSw.Stop();
            Console.WriteLine($"in \"{_tmpDb.FullPath}\" took {_createTempDatabaseSw.ElapsedMilliseconds} ms");

            _apphost = new AppHostBuilder()
                .UseDatabase(_tmpDb.FullPath)
                .UseSchedulerCount(schedulerCount)
                .Build();
        }

        public void Start()
        {
            Console.Write("Starting database ");
            _startingDatabaseSw.Start();
            _apphost.Start(); // Do this separately so _apphost is set on Exceptions
            _startingDatabaseSw.Stop();
            Console.WriteLine($"took {_startingDatabaseSw.ElapsedMilliseconds} ms");
            var qp = Starcounter.Nova.Db.GetCurrentQueryProcessor();
            Console.WriteLine($"Using query processor \"{qp.Name}\" of type {qp?.GetType()?.ToString()}");
        }

        public virtual void Dispose()
        {
            Console.WriteLine($"Disposing TempAppHost");
            Interlocked.Exchange(ref _apphost, null).Dispose();
            Interlocked.Exchange(ref _tmpDb, null).Dispose();
        }
    }
}
