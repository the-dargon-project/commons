using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty
{
   public static partial class Extensions
   {
      /// <summary>
      /// Reads the given amount of characters, and treats
      /// the read characters as a string.  The string should 
      /// not be null terminated.
      /// </summary>
      public static string ReadStringOfLength(this BinaryReader reader, int length)
      {
         return Encoding.ASCII.GetString(reader.ReadBytes(length));
      }

      /// <summary>
      /// Reads bytes until we get to a NULL.  Treat bytes as characters for a string.
      /// The underlying stream is treated as UTF-8.
      /// </summary>
      /// <returns></returns>
      public static string ReadNullTerminatedString(this BinaryReader reader)
      {
         List<byte> dest = new List<byte>();
         byte b;
         while ((b = reader.ReadByte()) != 0)
         {
            dest.Add(b);
         }
         return Encoding.UTF8.GetString(dest.ToArray());
      }

      public static void WriteNullTerminatedString(this BinaryWriter writer, string value)
      {
         for (int i = 0; i < value.Length; i++)
         {
            writer.Write(value[i]);
         }
         writer.Write('\0');
      }

      //http://stackoverflow.com/questions/8613187/an-elegant-way-to-consume-all-bytes-of-a-binaryreader
      public static byte[] ReadAllBytes(this BinaryReader reader)
      {
         const int bufferSize = 4096;
         using (var ms = new MemoryStream())
         {
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
               ms.Write(buffer, 0, count);
            return ms.ToArray();
         }

      }
   }
}
