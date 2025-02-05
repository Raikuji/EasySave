using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EasyCmd.Model
{
    /// <summary>
    /// Class that represents the state of the backup work.
    /// </summary>
    internal class WorkState
    {
        private int _totalFiles;
        private long _totalSize;
        private int _remainingFiles;
        private long _remainingSize;

        /// <summary>
        /// Constructor of the WorkState class.
        /// </summary>
        public WorkState()
        {
            _totalFiles = 0;
            _totalSize = 0;
            _remainingFiles = 0;
            _remainingSize = 0;
        }

        /// <summary>
        /// Sets the total number of files and total size of the files to copy.
        /// </summary>
        /// <param name="totalFiles"></param>
        /// <param name="totalSize"></param>
        public void SetTotal(int totalFiles, long totalSize)
        {
            _totalFiles = totalFiles;
            _totalSize = totalSize;
            _remainingFiles = totalFiles;
            _remainingSize = totalSize;
        }

        /// <summary>
        /// Updates the remaining number of files and size of the files to copy.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="size"></param>
        public void UpdateRemaining(int files, long size)
        {
            _remainingFiles = files;
            _remainingSize = size;
        }

        /// <summary>
        /// Returns the total number of files.
        /// </summary>
        /// <returns></returns>
        public int GetTotalFiles()
        {
            return _totalFiles;
        }

        /// <summary>
        /// Returns the total size of the files.
        /// </summary>
        /// <returns></returns>
        public long GetTotalSize()
        {
            return _totalSize;
        }

        /// <summary>
        /// Returns the remaining number of files.
        /// </summary>
        /// <returns></returns>
        public int GetRemainingFiles()
        {
            return _remainingFiles;
        }

        /// <summary>
        /// Returns the remaining size of the files.
        /// </summary>
        /// <returns></returns>
        public long GetRemainingSize()
        {
            return _remainingSize;
        }
    }
}
