using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupWorkFull : IBackupWorkStrategy
    {
        public void Execute(BackupJob backupJob, string source, string destination)
        {
            // Get the total size and number of all files to copy
            long totalSize = GetTotalSizeOfFiles(source);
            int totalFiles = GetTotalNumberOfFiles(source);
            long remainingSize = totalSize;
            int remainingFiles = totalFiles;

            // Recreate all the directories
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(source, destination));
            }

            // Copy all the files
            foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                string destFilePath = filePath.Replace(source, destination);
                File.Copy(filePath, destFilePath, true);

                // Update the remaining size and file count
                FileInfo fileInfo = new FileInfo(filePath);
                remainingSize -= fileInfo.Length;
                remainingFiles--;

                // Update remaining size and file count in the backup job
                backupJob.UpdateWorkState(remainingFiles, remainingSize, filePath, destFilePath);
            }
        }

        public long GetTotalSizeOfFiles(string source)
        {
            long totalSize = 0;
            foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                totalSize += fileInfo.Length;
            }
            return totalSize;
        }
        public int GetTotalNumberOfFiles(string source)
        {
            int totalFiles = 0;
            foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                totalFiles++;
            }
            return totalFiles;
        }
    }
}
