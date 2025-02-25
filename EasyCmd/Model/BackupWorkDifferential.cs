namespace EasyCmd.Model
{
	/// <summary>
	/// Class that represents the backup work strategy for differential backup.
	/// </summary>
	internal class BackupWorkDifferential : IBackupWorkStrategy
	{
		/// <summary>
		/// Executes a differential backup work.
		/// </summary>
		/// <param name="backupJob"></param>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public void Execute(BackupJob backupJob, string source, string destination)
		{
			// Get the total size and number of all files to copy
			long totalSize = GetTotalSizeOfFiles(source, destination);
			int totalFiles = GetTotalNumberOfFiles(source, destination);
			long remainingSize = totalSize;
			int remainingFiles = totalFiles;

			// Set the total number of files and total size of the files to copy
			backupJob.SetTotalWorkState(totalFiles, totalSize);

			// Recreate all the directories
			foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(source, destination));
			}

			// Copy only the modified files
			foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
			{
				if (!backupJob.IsRunning)
				{
					break;
				}
				if (backupJob.IsPaused)
				{
					backupJob.PauseEvent.Wait();
				}

				string destFilePath = filePath.Replace(source, destination);
				FileInfo sourceFileInfo = new FileInfo(filePath);
				FileInfo destFileInfo = new FileInfo(destFilePath);

				if (!destFileInfo.Exists || sourceFileInfo.LastWriteTime > destFileInfo.LastWriteTime)
				{
					DateTime transfertStart = DateTime.Now;
					try
					{
						File.Copy(filePath, destFilePath, true);
						int encryptionTime = BackupJob.EncryptFile(filePath);

						// Update the remaining size and file count
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
			}
		}

		/// <summary>
		/// Gets the total size of all files in the source directory.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		public long GetTotalSizeOfFiles(string source, string destination)
		{
			long totalSize = 0;
			foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
			{
				string destFilePath = filePath.Replace(source, destination);
				FileInfo sourceFileInfo = new FileInfo(filePath);
				FileInfo destFileInfo = new FileInfo(destFilePath);

				if (!destFileInfo.Exists || sourceFileInfo.LastWriteTime > destFileInfo.LastWriteTime)
				{
					totalSize += sourceFileInfo.Length;
				}
			}
			return totalSize;
		}

		/// <summary>
		/// Gets the total number of files in the source directory.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		public int GetTotalNumberOfFiles(string source, string destination)
		{
			int totalFiles = 0;
			foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
			{
				string destFilePath = filePath.Replace(source, destination);
				FileInfo sourceFileInfo = new FileInfo(filePath);
				FileInfo destFileInfo = new FileInfo(destFilePath);

				if (!destFileInfo.Exists || sourceFileInfo.LastWriteTime > destFileInfo.LastWriteTime)
				{
					totalFiles++;
				}
			}
			return totalFiles;
		}
	}
}