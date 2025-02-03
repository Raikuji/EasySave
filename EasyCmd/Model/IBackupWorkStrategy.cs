using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    internal interface IBackupWorkStrategy
    {
        void Execute(string source, string destination);
    }
}
