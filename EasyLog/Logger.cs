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
        

        
        /// Méthode pour écrire un log. 
        public void LogAction(
            string backupName,
            string sourceFilePath,
            string destinationFilePath,
            string backupType,
            long fileSize,
            long transferTimeMs)
        {

            // 1) Créer une bibliothéque de données pour le log
            var logData = new Dictionary<string, object>
            {
                ["Name"] = "Save1",
                ["FileSize"] = fileSize,
                ["Timestamp"] = DateTime.Now
                ["transferTimeMs"] = transferTimeMs,
                ["BackupType"] = backupType;
                ["sourceFilePath"] = sourceFilePath,
                ["destinationFilePath"] = destinationFilePath
            };

            // 2) Générer le nom du fichier journalier (exemple : 2023-02-03.json)
            string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            string logFilePath = Path.Combine(_logDirectory, logFileName);

            // 3) Sérialiser l’objet en JSON
            string json = JsonSerializer.Serialize(logData);

            // 4) Écrire le JSON dans le fichier, suivi d’un retour à la ligne
            //    Ouvrir en append pour ne pas écraser le contenu existant
            
                File.AppendAllText(myLogFile, json + Environment.NewLine);
            
        }
       
}
