using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RadioBot {
    internal class Utils {
        internal static string GetToken() {
            return File.ReadAllText("C:\\Data\\DiscordBots\\Authorization\\RadioBot.auth");
        }
    }
}
