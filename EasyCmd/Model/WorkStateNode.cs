﻿
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;

namespace EasyCmd.Model
{
    /// <summary>
    /// Class that represents the state of the backup work.
    /// </summary>
    internal class WorkStateNode
    {
        public required string Name { get; set; }
        public required string SourceFilePath { get; set; }
        public required string DestinationFilePath { get; set; }
        public required string State { get; set; }
        public int TotalFilesToCopy { get; set; }
        public long TotalFileSizeToCopy { get; set; }
        public int RemainingFilesToCopy { get; set; }
        public long Progression { get; set; }
        public DateTime LastUpdate { get; set; }
        public static string LOGPATH = AppDomain.CurrentDomain.BaseDirectory + "\\log";
        public static string WORKSTATENAME = "work_state.json";

        private static Mutex _mutex = new Mutex();

		/// <summary>
		/// Adds or updates a work state node in the work_state.json file.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <param name="workState"></param>
		public static void AddOrUpdateWorkStateNode(string name, string source, string destination, WorkState workState)
        {
			_mutex.WaitOne();
			var obj = new WorkStateNode
            {
                Name = name,
                SourceFilePath = workState.GetRemainingFiles() == 0 ? "" : source,
                DestinationFilePath = workState.GetRemainingFiles() == 0 ? "" : destination,
                State = workState.GetRemainingFiles() == 0 ? "END" : "ACTIVE",
                TotalFilesToCopy = workState.GetTotalFiles(),
                TotalFileSizeToCopy = workState.GetTotalSize(),
                RemainingFilesToCopy = workState.GetRemainingFiles(),
                Progression = workState.GetRemainingSize(),
                LastUpdate = DateTime.Now
            };

            string filePath = LOGPATH + "\\" + WORKSTATENAME;
            List<WorkStateNode> workStates;

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() },
                    PropertyNameCaseInsensitive = true
                };
                workStates = JsonSerializer.Deserialize<List<WorkStateNode>>(json, options) ?? new List<WorkStateNode>();
            }
            else
            {
                workStates = new List<WorkStateNode>();
            }

            var existingWorkState = workStates.FirstOrDefault(ws => ws.Name == name);
            if (existingWorkState != null)
            {
                existingWorkState.SourceFilePath = obj.SourceFilePath;
                existingWorkState.DestinationFilePath = obj.DestinationFilePath;
                existingWorkState.State = obj.State;
                existingWorkState.TotalFilesToCopy = obj.TotalFilesToCopy;
                existingWorkState.TotalFileSizeToCopy = obj.TotalFileSizeToCopy;
                existingWorkState.RemainingFilesToCopy = obj.RemainingFilesToCopy;
                existingWorkState.Progression = obj.Progression;
                existingWorkState.LastUpdate = obj.LastUpdate;
            }
            else
            {
                workStates.Add(obj);
            }

            string updatedJson = JsonSerializer.Serialize(workStates, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
			_mutex.ReleaseMutex();
		}

        /// <summary>
        /// Removes a work state node from the work_state.json file.
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveWorkStateNode(string name)
        {
            _mutex.WaitOne();
			string filePath = LOGPATH + "\\" + WORKSTATENAME;
            List<WorkStateNode> workStates;
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() },
                    PropertyNameCaseInsensitive = true
                };
                workStates = JsonSerializer.Deserialize<List<WorkStateNode>>(json, options) ?? new List<WorkStateNode>();
            }
            else
            {
                workStates = new List<WorkStateNode>();
            }
            var existingWorkState = workStates.FirstOrDefault(ws => ws.Name == name);
            if (existingWorkState != null)
            {
                workStates.Remove(existingWorkState);
            }
            string updatedJson = JsonSerializer.Serialize(workStates, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
			_mutex.ReleaseMutex();
		}
    }
}