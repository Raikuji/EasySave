using System.Diagnostics;

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
            // Get priority extensions
            var priorityExtensions = Settings.GetInstance().PriorityExtensions.Select(ext => ext.ToLower()).ToList();
            // Look for the extensions
            var allFiles = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
            //Priority Files List
            var priorityFiles = allFiles.Where(file => priorityExtensions.Contains(Path.GetExtension(file).ToLower())).ToList();
            //Standard Files List
            var standardFiles = allFiles.Except(priorityFiles).ToList();
            // Get the total size and number of all files to copy
            long totalSize = GetTotalSizeOfFiles(source);
            int totalFiles = GetTotalNumberOfFiles(source);
            long remainingSize = totalSize;
            int remainingFiles = totalFiles;
            
            // Set the total number of files and total size of the files to copy
			backupJob.SetTotalWorkState(totalFiles, totalSize);

            // Recreate all the directories
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                string destPath = dirPath.Replace(source, destination);
                Directory.CreateDirectory(destPath);
                backupJob.Log(dirPath, destPath, 0, DateTime.Now, 0);
            }
            
            //Copy of the priority files first
            foreach (string filePath in priorityFiles)
            {
                DateTime transfertStart = DateTime.Now;
                string destFilePath = filePath.Replace(source, destination);

                try
                {
                    File.Copy(filePath, destFilePath, true);
                    int encryptionTime = BackupJob.EncryptFile(destFilePath);
                    // Update the remaining size and file count
                    FileInfo sourceFileInfo = new FileInfo(filePath);
                    remainingSize -= sourceFileInfo.Length;
                    remainingFiles--;

                    // Update remaining size and file count in the backup job
                    backupJob.UpdateWorkState(remainingFiles, remainingSize, filePath, destFilePath);
                    backupJob.Log(filePath, destFilePath, sourceFileInfo.Length, transfertStart, encryptionTime);
                }
                catch (Exception)
                {
                    backupJob.Log(filePath, destFilePath, -1, transfertStart, -1);
                }
            }


            // Copy all the files
            foreach (string filePath in standardFiles)
            {
				if (!backupJob.IsRunning)
				{
					break;
				}
				if (backupJob.IsPaused)
				{
					backupJob.PauseEvent.Wait();
				}

				DateTime transfertStart = DateTime.Now;
				string destFilePath = filePath.Replace(source, destination);
				try
				{
					File.Copy(filePath, destFilePath, true);
					int encryptionTime = BackupJob.EncryptFile(destFilePath);

					// Update the remaining size and file count
					FileInfo sourceFileInfo = new FileInfo(filePath);
					remainingSize -= sourceFileInfo.Length;
					remainingFiles--;

					// Update remaining size and file count in the backup job
					backupJob.UpdateWorkState(remainingFiles, remainingSize, filePath, destFilePath);
					backupJob.Log(filePath, destFilePath, sourceFileInfo.Length, transfertStart, encryptionTime);
				}
				catch (Exception)
				{
					backupJob.Log(filePath, destFilePath, -1, transfertStart, -1);
				}
			}
			backupJob.Stop();
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
