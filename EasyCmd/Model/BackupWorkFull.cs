using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupWorkFull : IBackupWorkStrategy
    {
        public void Execute(string source, string destination)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException();
            }
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException(source);
            }
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
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
            }
        }
    }
}
