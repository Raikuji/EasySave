using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRemote.Models; 


namespace EasyRemote.Models
{
    public class BackupJob
    {
        public string Name { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
    }
}

