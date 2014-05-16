using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace ItzWarty
{
    /// <summary>
    /// This class can store development information in a file, to count how many times we build.
    /// deprecated/no longer used in favor of RAFManager's way of versioning.
    /// </summary>
    public static class DevInfo
    {
        public static int major = 0;        //Huge changes
        public static int minor = 0;        //bugs fixed
        public static int revision = 0;     //Smaller bugs fixed
        public static string flags = "b";
        public static int build = -1;

        public static string version
        {
            get
            {
                return ToString();
            }
        }
        public new static string ToString()
        {
            return major + "." + minor + "." + revision + flags + " build:" + build;
        }

        public static void Manage()
        {
            try
            {
                string execContent = System.IO.File.ReadAllText(Application.ExecutablePath);
                byte[] myHash = System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(execContent));
                string md5hash = BitConverter.ToString(myHash).Replace("-", "");

                string md5storage = @"..\..\.lastmd5hash"; //Go to the folder of .csproj
                string buildNumStorage = @"..\..\.build";
                string timestampStorage = @"..\..\.timestamp";

                bool incrementBuild = false;
                if (System.IO.File.Exists(md5storage))
                {
                    string lastmd5hash = System.IO.File.ReadAllText(md5storage);
                    if (md5hash != lastmd5hash)
                        incrementBuild = true;
                }
                else
                {
                    incrementBuild = true;
                }

                if (incrementBuild)
                {
                    System.IO.File.WriteAllText(md5storage, md5hash); //store the current build into build
                    System.IO.File.WriteAllText(timestampStorage, DateTime.Now.ToLongDateString() + " @ " + DateTime.Now.ToLongTimeString()); //store the current build into build

                    if (System.IO.File.Exists(buildNumStorage))
                    {
                        System.IO.File.WriteAllText(buildNumStorage,
                            (Int32.Parse(System.IO.File.ReadAllText(buildNumStorage)) + 1).ToString()   //Read the content, parse, increment, store
                        );
                    }
                    else
                    {
                        System.IO.File.WriteAllText(buildNumStorage, "1"); //we be the first build.
                    }
                }

                build = Int32.Parse(System.IO.File.ReadAllText(buildNumStorage));
            }
            catch
            {
                major = -1; minor = -1; build = -1; flags = "????"; build = -1;
            }
        }
    }
}
