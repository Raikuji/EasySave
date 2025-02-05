using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    /// <summary>
    /// Class that represents the full backup work strategy.
    /// </summary>
    internal class BackupWorkFull : IBackupWorkStrategy
    {
        /// <summary>
        /// Executes a full backup work.
        /// </summary>
        /// <param name="backupJob"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
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

        /// <summary>
        /// Gets the total size of all files in the source directory.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the total number of files in the source directory.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
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
