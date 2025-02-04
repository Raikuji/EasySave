using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal class BackupJob
    {
        private string _name { get; }
        private string _source { get; }
        private string _destination { get; }
        private IBackupWorkStrategy _backupStrategy;

        public BackupJob(string name, string source, string destination, IBackupWorkStrategy backupStrategy)
        {
            _name = name;
            _source = source;
            _destination = destination;
            _backupStrategy = backupStrategy;
        }
        public BackupJob(string name, string source, string destination, int strategyId)
        {
            _name = name;
            _source = source;
            _destination = destination;
            _backupStrategy = GetBackupStrategy(strategyId);
        }
        public IBackupWorkStrategy GetBackupStrategy(int strategyId)
        {
            switch (strategyId)
            {
                case 1:
                    return new BackupWorkFull();
                case 2:
                    return new BackupWorkDifferential();
                default:
                    throw new ArgumentException("Invalid strategy ID");
            }
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
            return _name + " " + _source + " " + _destination + " " + _backupStrategy.GetType().Name;
        }
        public string ToJson()
        {
            dynamic obj = new ExpandoObject();
            obj.name = _name;
            obj.source = _source;
            obj.destination = _destination;
            obj.strategyId = GetStrategyId();
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
        public void Execute()
        {
            _backupStrategy.Execute(_source, _destination);
        }
    }
}
