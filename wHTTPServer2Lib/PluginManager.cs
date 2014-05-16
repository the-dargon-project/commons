using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO;

using ItzWarty;

using System.Reflection;

namespace ItzWarty.wHTTPServer2Lib
{
    public static class PluginManager
    {
        /// <summary>
        /// pluginName, pluginState
        /// </summary>
        private static Dictionary<string, IPlugin> pluginInstances = new Dictionary<string, IPlugin>();

        public static void Initialize()
        {
            LoadPluginDirectory(g.pluginDir);
        }
        private static void LoadPluginDirectory(string dir)
        {
            foreach (string directoryPath in Directory.GetDirectories(dir))
                if(!directoryPath.Contains("."))
                    LoadPluginDirectory(directoryPath);
            foreach (string filePath in Directory.GetFiles(dir))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Environment.CurrentDirectory = fileInfo.Directory.FullName;
                if (fileInfo.Extension == ".dll")
                {
                    Assembly assembly = wPluginHelper.LoadPluginAssembly(filePath);
                    if(assembly != null)
                    {
                        try
                        {
                            IPlugin plugin = wPluginHelper.GetPluginInstance(assembly);
                            pluginInstances.Add(plugin.PluginName, plugin);
                        }
                        catch 
                        {
                            Console.WriteLine("Could not load " + filePath + ".  Maybe it isn't a plugin?");
                        }
                    }
                }
            }
        }
        public static IPlugin GetPluginInstance(string pluginName)
        {
            if (pluginInstances.ContainsKey(pluginName))
                return pluginInstances[pluginName];
            return null;
        }
    }
}
