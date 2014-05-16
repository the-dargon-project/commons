using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using System.Diagnostics;
using System.Reflection;

using System.Threading;
using System.IO;

namespace ItzWarty.Web
{
    public static class PluginManager
    {
        private static Dictionary<string, object> pluginGroupObjectCache = new Dictionary<string, object>();
        public static void Init()
        {
            InitPluginsInFolder(Environment.CurrentDirectory);
        }
        public static object getPluginState(string pluginCacheName)
        {
            if (new List<string>(pluginGroupObjectCache.Keys).Contains(pluginCacheName))
                return pluginGroupObjectCache[pluginCacheName];
            else
                return null;
        }
        private static void InitPluginsInFolder(string loc)
        {
            string[] files = Directory.GetFiles(loc);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension.ToLower() == ".w" ||
                    fileInfo.Extension.ToLower() == ".dll")
                {
                    Assembly assembly = Assembly.LoadFile(file);
                    foreach (Type asmType in assembly.GetTypes())
                    {
                        if (asmType.GetInterface("IPlugin") != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(asmType);
                            
                        }
                    }
                }
            }
        }
    }
}
