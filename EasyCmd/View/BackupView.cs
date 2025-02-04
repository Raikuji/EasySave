using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyCmd.ViewModel;

namespace EasyCmd.View
{
    internal class BackupView
    {
        public void Display(string message, string? jobs = null)
        {
            Console.WriteLine(message);
            if (jobs != null)
            {
                Console.WriteLine(jobs);
            }
        }
    }
}
