using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//This namespace is so simple that I don't even want to comment it.. it's annoying to comment, but everything's 
//straightforward... but hard to describe in text....
namespace ItzWarty.Database
{
    public static class wDB
    {
        private static string rootPath = "C:/wDB/";
        /// <summary>
        /// Gets the Databases of our WDB root path
        /// </summary>
        public static wDBDatabaseCollection Databases
        {
            get
            {
                return new wDBDatabaseCollection(rootPath);
            }
        }
        /// <summary>
        /// Gets or sets the root path of the WDB
        /// </summary>
        public static string RootPath
        {
            get
            {
                return rootPath;
            }
            set
            {
                rootPath = value;
            }
        }
    }
}
