using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.Database
{
    public class wDBDatabase
    {
        public string Path { get; private set; }
        public wDBDatabase(string path)
        {
            this.Path = path;
        }
        public wDBValueCollection Values
        {
            get
            {
                return new wDBValueCollection(Path);
            }
        }
        public wDBTableCollection Tables
        {
            get
            {
                return new wDBTableCollection(Path);
            }
        }
        public wDBDatabaseCollection Databases
        {
            get
            {
                return new wDBDatabaseCollection(Path);
            }
        }
    }
}
