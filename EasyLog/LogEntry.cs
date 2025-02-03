namespace EasyLog
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string BackupName { get; set; }
        public string SourceFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        public long FileSize { get; set; }
        public long TransferTimeMs { get; set; } // Négatif si erreur
    }
}