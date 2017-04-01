using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET.Services
{
    public class LogServices
    {
        private const string LogPath = @"./Logs/Log.txt";
        public static void WriteLog(string message)
        {
            using(StreamWriter file = File.AppendText(LogPath))
            {
                file.WriteLine(string.Format("{0} : {1}", DateTime.Now, message));
            }
        }

    }
}
