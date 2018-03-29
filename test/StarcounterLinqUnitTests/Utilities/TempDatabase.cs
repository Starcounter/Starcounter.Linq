using System;
using System.Threading;
using Starcounter.Nova.Bluestar;
using Starcounter.Nova.Options;

namespace StarcounterLinqUnitTests.Utilities
{
    /// <summary>
    /// Creates a temporary Starcounter database that is removed when Disposed.
    /// </summary>
    public class TempDatabase : IDisposable
    {
        private TempDirectory _tmpDir;

        public StarcounterOptions StarcounterOptions { get; private set; }
        public string FullPath { get { return _tmpDir.FullPath; } }
        public string DatabaseName { get { return _tmpDir.Name; } }

        public TempDatabase(string parentPath = null)
        {
            _tmpDir = new TempDirectory(parentPath);
            ScCreateDb.Execute(_tmpDir.FullPath);
            if (StarcounterOptions.TryOpenExisting(out StarcounterOptions options, _tmpDir.FullPath))
                StarcounterOptions = options;
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _tmpDir, null)?.Dispose();
        }
    }
}