using System;
using System.Dynamic;
using System.IO;
using System.Text.Json;
using EasyLog;
namespace EasyCmd.Model
{
    /// <summary>
    /// Represents a backup job.
    /// </summary>
    internal class BackupJob
    {
        private string _name { get; }
        private string _source { get; }
        private string _destination { get; }
        private IBackupWorkStrategy _backupStrategy;
        private WorkState _workState;
        private LogFormat _logFormat; 

        /// <summary>
        /// Constructor of the BackupJob class.
        /// </summary>
        public BackupJob(string name, string source, string destination, int strategyId, LogFormat logFormat)
        {
            _name = name;
            _source = source;
            _destination = destination;
            _backupStrategy = GetBackupStrategy(strategyId);
            _workState = new WorkState();
            _logFormat = logFormat; 
        }

        public string GetName() => _name;

        public IBackupWorkStrategy GetBackupStrategy(int strategyId)
        {
            return strategyId switch
            {
                1 => new BackupWorkFull(),
                2 => new BackupWorkDifferential(),
                _ => throw new ArgumentException("Invalid strategy ID")
            };
        }

        public int GetStrategyId()
        {
            return _backupStrategy switch
            {
                BackupWorkFull => 1,
                BackupWorkDifferential => 2,
                _ => throw new ArgumentException("Invalid strategy")
            };
        }

        public override string ToString()
        {
            return $"{_name} {_source} {_destination} {_backupStrategy.GetType().Name}";
        }

        public string ToJson()
        {
            dynamic obj = new ExpandoObject();
            obj.name = _name;
            obj.source = _source;
            obj.destination = _destination;
            obj.strategyId = GetStrategyId();
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        }

        /// <summary>
        /// Logs the backup job execution details.
        /// </summary>
        public void Log(string source, string destination, long size, DateTime transfertStart)
        {
            BackupJobLog backupJobLog = new BackupJobLog(
                _name,
                source,
                destination,
                size,
                (DateTime.Now - transfertStart).TotalSeconds,
                DateTime.Now,
                _logFormat.ToString() 
            );
            backupJobLog.Log();
        }

        public void SetTotalWorkState(int totalFiles, long totalSize)
        {
            _workState.SetTotal(totalFiles, totalSize);
            WorkStateNode.AddOrUpdateWorkStateNode(_name, _source, _destination, _workState);
        }

        public void UpdateWorkState(int files, long size, string currentFileSource, string currentFileDestination)
        {
            _workState.UpdateRemaining(files, size);
            WorkStateNode.AddOrUpdateWorkStateNode(_name, currentFileSource, currentFileDestination, _workState);
        }

        public WorkState GetWorkState() => _workState;

        public bool Execute()
        {
            bool success = true;
            try
            {
                if (string.IsNullOrWhiteSpace(_source) || string.IsNullOrWhiteSpace(_destination))
                {
                    throw new ArgumentNullException();
                }
                if (!Directory.Exists(_source))
                {
                    throw new DirectoryNotFoundException(_source);
                }
                if (!Directory.Exists(_destination))
                {
                    Directory.CreateDirectory(_destination);
                }
                _backupStrategy.Execute(this, _source, _destination);
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                UpdateWorkState(0, 0, "", "");
            }
            return success;
        }
    }
}
