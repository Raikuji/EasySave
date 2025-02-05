using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyCmd.ViewModel;

namespace EasyCmd.View
{
    /// <summary>
    /// Class that represents the view of the backup.
    /// </summary>
    internal class BackupView
    {
        /// <summary>
        /// Displays the message and the jobs list.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="jobs"></param>
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
