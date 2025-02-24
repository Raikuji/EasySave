using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EasySave.Model
{
    public class BackupManager
    {
        // List to track running backup tasks
        private List<Task> backupTasks = new List<Task>();

        // Lock object to synchronize access to shared resources
        private object lockObject = new object();

        /// <summary>
        /// Starts the backup process for a list of backup jobs.
        /// </summary>
        /// <param name="jobs">List of backup jobs to execute.</param>
        public void StartBackup(List<BackupJob> jobs)
        {
            Console.WriteLine("Starting parallel backups...");

            // Execute each job in parallel
            Parallel.ForEach(jobs, job =>
            {
                Task backupTask = Task.Run(() => RunBackup(job));

                // Add the task to the list in a thread-safe manner
                lock (lockObject)
                {
                    backupTasks.Add(backupTask);
                }
            });

            // Wait for all backup tasks to complete
            Task.WhenAll(backupTasks).Wait();
            Console.WriteLine("All backups are completed.");
        }

        /// <summary>
        /// Runs a single backup job.
        /// </summary>
        /// <param name="job">The backup job to execute.</param>
        private void RunBackup(BackupJob job)
        {
            Console.WriteLine($"[START] Backing up from {job.Source} to {job.Destination}");

            try
            {
                // Ensure the destination directory exists
                Directory.CreateDirectory(job.Destination);

                // Copy each file from source to destination
                foreach (var file in Directory.GetFiles(job.Source))
                {
                    string destFile = Path.Combine(job.Destination, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                    Console.WriteLine($"[OK] {file} -> {destFile}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Backup failed: {ex.Message}");
            }

            Console.WriteLine($"[END] Backup completed: {job.Source} to {job.Destination}");
        }
    }

    /// <summary>
    /// Represents a backup job with source and destination paths.
    /// </summary>
    public class BackupJob
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public BackupJob(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}