using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace EngagerMark4.Utils
{
    /// <summary>
    /// Aung Ye Kaung - 20191030 - Version Utility
    /// </summary>
    public static class VersionUtil
    {
        public static string GetVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}