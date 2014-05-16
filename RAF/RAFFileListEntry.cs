using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.RAF
{
    public class RAFFileListEntry
    {
        private byte[] directoryFileContent = null;
        private UInt32 offsetEntry = 0;
        private RAF raf = null;
        public RAFFileListEntry(RAF raf, byte[] directoryFileContent, UInt32 offsetEntry)
        {
            this.raf = raf;
            this.directoryFileContent = directoryFileContent;
            this.offsetEntry = offsetEntry;
        }
        /// <summary>
        /// Hash of the string name
        /// </summary>
        public UInt32 StringNameHash
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry);
            }
        }
        /// <summary>
        /// Offset to the start of the archived file in the data file
        /// </summary>
        public UInt32 FileOffset
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry+4);
            }
        }
        /// <summary>
        /// Size of this archived file
        /// </summary>
        public UInt32 FileSize
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry+8);
            }
        }
        /// <summary>
        /// Index of the name of the archvied file in the string table
        /// </summary>
        public UInt32 FileNameStringTableIndex
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry+12);
            }
        }

        public String GetFileName
        {
            get
            {
                return this.raf.GetDirectoryFile().GetStringTable()[this.FileNameStringTableIndex];
            }
        }
    }
}
