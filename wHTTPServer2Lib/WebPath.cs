using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using ItzWarty;

namespace ItzWarty.wHTTPServer2Lib
{
    public static class WebPath
    {
        /// <summary>
        /// Resolves our web-path, following symbolic links and such
        /// </summary>
        /// <param name="webPath"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string ResolvePath(string webPath, string host)
        {
            Console.WriteLine("Resolving: " + webPath+" @ "+host);
            string currentDirectory = g.webDir+host+"/";
            Stack<string> breadCrumbs = new Stack<string>();
            string[] pathParts = webPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
            for (int i = pathParts.Length - 1; i >= 0; i--)
                breadCrumbs.Push(pathParts[i]);

            while (breadCrumbs.Count > 0)
            {
                string fsoName = breadCrumbs.Pop();
                Console.WriteLine("Current fsoName: " + fsoName);
                if (Directory.Exists(currentDirectory + fsoName))
                {
                    Console.WriteLine("  is dir");
                    currentDirectory += fsoName + "/";
                }
                else if (File.Exists(currentDirectory + ".^r" + fsoName))
                {   //relative symbolic link
                    Console.WriteLine("  is rdir");
                    string[] symLinkPathParts = File.ReadAllText(currentDirectory + ".^r" + fsoName).Split("/");
                    for (int i = symLinkPathParts.Length - 1; i >= 0; i--)
                        breadCrumbs.Push(symLinkPathParts[i]);
                }
                else if (File.Exists(currentDirectory + ".^a" + fsoName))
                {
                    Console.WriteLine("  is adir");
                    currentDirectory = g.webDir + File.ReadAllText(currentDirectory + ".^r" + fsoName);
                }else if(File.Exists(currentDirectory + fsoName))
                {
                    Console.WriteLine("  is file");
                    currentDirectory += fsoName;
                }
                else
                {
                    Console.WriteLine("  doesn't exist at" + currentDirectory + " fsoName: " + fsoName);
                    return null;
                }
            }

            string resolvedLocalPath = currentDirectory;
            //if (breadCrumbs.Count == 1) resolvedLocalPath += breadCrumbs.Pop();
            if (Directory.Exists(resolvedLocalPath)) resolvedLocalPath += "/";

            resolvedLocalPath = StripDoubleSlash(resolvedLocalPath);
            resolvedLocalPath = "/"+resolvedLocalPath.Substring(g.webDir.Length, resolvedLocalPath.Length - g.webDir.Length);
            return resolvedLocalPath.Substring((host+"/").Length, resolvedLocalPath.Length - (host+"/").Length);
        }
        public static string StripDoubleSlash(String s)
        {
            return System.Text.RegularExpressions.Regex.Replace(s, "/+", "/");
        }
    }
}
