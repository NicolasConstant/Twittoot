using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Twittoot.Common
{
    public class TwittootLocation
    {
        public static string GetExecutingAsmLocation()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static string GetUserDataFolder()
        {
            return "PersonnalSyncData";
        }

        public static string GetUserFilePath(string fileName)
        {
            var executingAsmDir = GetExecutingAsmLocation();
            var userDataFolder = GetUserDataFolder();
            var dirFullPath = Path.Combine(executingAsmDir, userDataFolder);
            if (!Directory.Exists(dirFullPath)) Directory.CreateDirectory(dirFullPath);
            return Path.Combine(executingAsmDir, userDataFolder, fileName);
        }
    }
}
