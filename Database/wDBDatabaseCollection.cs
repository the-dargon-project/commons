using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ItzWarty.Database
{
    public class wDBDatabaseCollection
    {
        private string path = "";
        public wDBDatabaseCollection(string path)
        {
            this.path = path;
        }
        public wDBDatabase this[string s]
        {
            get
            {
                return new wDBDatabase(path + "#db_" + s + "/"); //This might throw
            }
        }
        public string[] EnumerateDatabases()
        {
            string[] matches = Directory.GetDirectories(path + "/", "#db_*");
            string[] result = new string[matches.Length];
            for (int i = 0; i < matches.Length; i++)
                result[i] = new DirectoryInfo(matches[i]).Name.Substring(4); //Remove #db_
            return result;
        }
    }
}
