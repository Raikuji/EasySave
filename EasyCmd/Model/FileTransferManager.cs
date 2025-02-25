using System;
using System.IO;
using System.Threading;

namespace EasyCmd.Model
{
    public class FileTransferManager
    {
        private readonly object _lock = new object();
        private bool _isLargeFileTransferring = false;

        public void TransferFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            int maxParallelFileSizeKB = Settings.GetInstance().MaxParallelFileSizeKB;

            lock (_lock)
            {
                // Check if the file size exceeds the maximum allowed size for parallel transfer
                if (fileInfo.Length > maxParallelFileSizeKB * 1024)
                {
                    // Wait until the current large file transfer is complete
                    while (_isLargeFileTransferring)
                    {
                        Monitor.Wait(_lock);
                    }
                    _isLargeFileTransferring = true;
                }
            }

            try
            {
                // File transfer logic
                // ...

                // Simulate file transfer
                Console.WriteLine($"Transferring file: {filePath}");
                Thread.Sleep(1000); // Simulate transfer delay
            }
            finally
            {
                lock (_lock)
                {
                    // After transfer, reset the flag and notify waiting threads
                    if (fileInfo.Length > maxParallelFileSizeKB * 1024)
                    {
                        _isLargeFileTransferring = false;
                        Monitor.PulseAll(_lock);
                    }
                }
            }
        }
    }
}