using System;
using System.IO;
using System.Threading;

namespace EasyCmd.Model
{
    public class FileTransferManager
    {
        private readonly object _lock = new object();
        private bool _isLargeFileTransferring = false;

        public void TransferFile(string filePath, long maxFileSizeInKB)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long fileSizeInKB = fileInfo.Length / 1024;

            lock (_lock)
            {
                if (fileSizeInKB > maxFileSizeInKB)
                {
                    while (_isLargeFileTransferring)
                    {
                        Monitor.Wait(_lock);
                    }
                    _isLargeFileTransferring = true;
                }
            }

            try
            {
                // Logic to transfer the file
                Console.WriteLine($"Transferring file: {filePath}");
                // Simulate file transfer
                Thread.Sleep(1000);
            }
            finally
            {
                lock (_lock)
                {
                    if (fileSizeInKB > maxFileSizeInKB)
                    {
                        _isLargeFileTransferring = false;
                        Monitor.PulseAll(_lock);
                    }
                }
            }
        }
    }
}