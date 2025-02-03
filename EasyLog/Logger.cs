using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using system.text.json;



namespace EasyLog
{
    public class Logger
    {
        private readonly string _logDirectory;  // Chemin du dossier où stocker les logs

        // Constructeur qui reçoit le chemin du dossier de logs
        public Logger(string logDirectory)
        {
            // On peut faire un check et créer le répertoire s’il n’existe pas
            _logDirectory = logDirectory;
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        /// <summary>
        /// Méthode pour écrire un log. 
        /// On peut soit passer directement un objet LogEntry, soit quelques paramètres bruts.
        /// </summary>
        public void LogAction(
            string backupName,
            string sourceFilePath,
            string destinationFilePath,
            long fileSize,
            long transferTimeMs)
        {
            // 1) Créer un objet LogEntry
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                BackupName = backupName,
                SourceFilePath = sourceFilePath,
                DestinationFilePath = destinationFilePath,
                FileSize = fileSize,
                TransferTimeMs = transferTimeMs
            };

            // 2) Générer le nom du fichier journalier (exemple : 2023-02-03.json)
            string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            string logFilePath = Path.Combine(_logDirectory, logFileName);

            // 3) Sérialiser l’objet en JSON
            //    Indenter n’est pas forcément nécessaire pour un log, 
            //    mais vous pouvez utiliser new JsonSerializerOptions { WriteIndented = true } si besoin
            //string json = JsonSerializer.Serialize(entry);

            // 4) Écrire le JSON dans le fichier, suivi d’un retour à la ligne
            //    Ouvrir en append pour ne pas écraser le contenu existant
            //using (var writer = new StreamWriter(logFilePath, append: true))
            //{
             //   writer.WriteLine(json);
            //}
        }
}
