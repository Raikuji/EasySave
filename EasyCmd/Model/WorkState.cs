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
    internal class WorkState
    {
        private int _totalFiles;
        private long _totalSize;
        private int _remainingFiles;
        private long _remainingSize;

        public WorkState()
        {
            _totalFiles = 0;
            _totalSize = 0;
            _remainingFiles = 0;
            _remainingSize = 0;
        }
        public void SetTotal(int totalFiles, long totalSize)
        {
            _totalFiles = totalFiles;
            _totalSize = totalSize;
            _remainingFiles = totalFiles;
            _remainingSize = totalSize;
        }
        public void UpdateRemaining(int files, long size)
        {
            _remainingFiles = files;
            _remainingSize = size;
        }
        public int GetTotalFiles()
        {
            return _totalFiles;
        }
        public long GetTotalSize()
        {
            return _totalSize;
        }
        public int GetRemainingFiles()
        {
            return _remainingFiles;
        }
        public long GetRemainingSize()
        {
            return _remainingSize;
        }
    }
}
