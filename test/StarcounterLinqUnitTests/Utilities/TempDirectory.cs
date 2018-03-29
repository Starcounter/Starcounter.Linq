using System;
using System.IO;

namespace StarcounterLinqUnitTests.Utilities
{
    public class TempDirectory : IDisposable
    {
        public readonly string FullPath;
        public readonly string Name;
        private bool _deleteOnDispose;

        public TempDirectory(string parentPath = null)
        {
            Name = Path.GetRandomFileName();
            FullPath = Path.Combine(parentPath ?? Path.GetTempPath(), Name);
            if (File.Exists(FullPath))
                throw new IOException(FullPath);
            if (!Directory.Exists(FullPath))
            {
                _deleteOnDispose = true;
                Directory.CreateDirectory(FullPath);
            }
        }

        public void Dispose()
        {
            if (_deleteOnDispose)
            {
                _deleteOnDispose = false;
                for (int attempts = 10; attempts > 0; attempts--)
                {
                    if (!Directory.Exists(FullPath))
                        break;
                    try
                    {
                        Directory.Delete(FullPath, true);
                    }
                    catch (IOException)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}