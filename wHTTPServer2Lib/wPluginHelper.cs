using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using ItzWarty.wHTTPServer2Lib;

namespace ItzWarty.wHTTPServer2Lib
{
    public class wPluginHelper
    {
        public static Assembly LoadPluginAssembly(string path)
        {
            try
            {
                return Assembly.LoadFrom(path);
            }
            catch
            {
                return null;
            }
        }
        public static IPlugin GetPluginInstance(Assembly assembly)
        {
            foreach(Type type in assembly.GetTypes())
            {
                if(type.GetInterface("IPlugin") != null)
                {
                    return (IPlugin)Activator.CreateInstance(type);
                }
            }
            return null;
        }
    }
}
