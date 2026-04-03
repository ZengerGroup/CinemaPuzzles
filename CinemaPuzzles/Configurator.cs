using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal static class Configurator
    {
        public static string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static string ErrorBin = ConfigurationManager.AppSettings["ErrorBin"];
        public static string TravelerOutput = ConfigurationManager.AppSettings["TravelerOutput"];
        public static string ReportPath = ConfigurationManager.AppSettings["ReportPath"];
    }
}
