using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupWorkDifferential : IBackupWorkStrategy
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

            DateTime lastBackupTime = GetLastBackupTime(destination);

            // Copy all the modified or new directories
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                string destDirPath = dirPath.Replace(source, destination);
                if (!Directory.Exists(destDirPath) || Directory.GetLastWriteTime(dirPath) > lastBackupTime)
                {
                    Directory.CreateDirectory(destDirPath);
                }
            }

            // Copy all the modified or new files
            foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                string destFilePath = filePath.Replace(source, destination);
                if (!File.Exists(destFilePath) || File.GetLastWriteTime(filePath) > lastBackupTime)
                {
                    File.Copy(filePath, destFilePath, true);
                }
            }
        }

        private DateTime GetLastBackupTime(string destination)
        {
            // Implémentez la logique pour obtenir l'heure de la dernière sauvegarde
            // Par exemple, vous pouvez lire cette information à partir d'un fichier de métadonnées
            // Pour cet exemple, nous retournons simplement la date et l'heure actuelles moins un jour
            return DateTime.Now.AddDays(-1);
        }
    }
}
