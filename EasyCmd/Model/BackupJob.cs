using System.Dynamic;
using System.Text.Json;

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

        /// <summary>
        /// Constructor of the BackupJob class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="strategyId"></param>
        public BackupJob(string name, string source, string destination, int strategyId)
        {
            _name = name;
            _source = source;
            _destination = destination;
            _backupStrategy = GetBackupStrategy(strategyId);
            _workState = new WorkState();
        }

        /// <summary>
        /// Returns the name of the backup job.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Returns the source of the backup job.
        /// </summary>
        /// <param name="strategyId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IBackupWorkStrategy GetBackupStrategy(int strategyId)
        {
            return strategyId switch
            {
                1 => new BackupWorkFull(),
                2 => new BackupWorkDifferential(),
                _ => throw new ArgumentException("Invalid strategy ID")
            };
        }

        /// <summary>
        /// Returns the strategy ID of the backup job.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int GetStrategyId()
        {
            return _backupStrategy switch
            {
                BackupWorkFull => 1,
                BackupWorkDifferential => 2,
                _ => throw new ArgumentException("Invalid strategy")
            };
        }

        /// <summary>
        /// Returns a string representation of the backup job.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_name} {_source} {_destination} {_backupStrategy.GetType().Name}";
        }

        /// <summary>
        /// Returns a JSON representation of the backup job.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            dynamic obj = new ExpandoObject();
            obj.name = _name;
            obj.source = _source;
            obj.destination = _destination;
            obj.strategyId = GetStrategyId();
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        }

        public void Log(string source, string destination, long size, DateTime transfertStart)
        {
            BackupJobLog backupJobLog = new BackupJobLog(_name, source, destination, size, (DateTime.Now - transfertStart).TotalSeconds, DateTime.Now,);
            backupJobLog.Log();
        }

        /// <summary>
        /// Sets the total work state of the backup job.
        /// </summary>
        /// <param name="totalFiles"></param>
        /// <param name="totalSize"></param>
        public void SetTotalWorkState(int totalFiles, long totalSize)
        {
            _workState.SetTotal(totalFiles, totalSize);
            WorkStateNode.AddOrUpdateWorkStateNode(_name, _source, _destination, _workState);
        }

        /// <summary>
        /// Updates the work state of the backup job.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="size"></param>
        /// <param name="currentFileSource"></param>
        /// <param name="currentFileDestination"></param>
        public void UpdateWorkState(int files, long size, string currentFileSource, string currentFileDestination)
        {
            _workState.UpdateRemaining(files, size);
            WorkStateNode.AddOrUpdateWorkStateNode(_name, currentFileSource, currentFileDestination, _workState);
        }

        /// <summary>
        /// Returns the work state of the backup job.
        /// </summary>
        /// <returns></returns>
        public WorkState GetWorkState()
        {
            return _workState;
        }

        /// <summary>
        /// Executes the backup job.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
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
