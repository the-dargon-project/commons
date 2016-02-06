using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dargon.Commons
{
   public static partial class DargonCommonsExtensions
   {
      /// <summary>
      /// Reads the given amount of characters, and treats
      /// the read characters as a string.  The string should 
      /// not be null terminated.
      /// </summary>
      public static string ReadStringOfLength(this BinaryReader reader, int length) {
         var data = reader.ReadBytes(length);
         return Encoding.UTF8.GetString(data, 0, data.Length);
      }

      /// <summary>
      /// Reads bytes until we get to a NULL.  Treat bytes as characters for a string.
      /// The underlying stream is treated as UTF-8.
      /// </summary>
      /// <returns></returns>
      public static string ReadNullTerminatedString(this BinaryReader reader) {
         using (var ms = new MemoryStream()) {
            byte b;
            while ((b = reader.ReadByte()) != 0) {
               ms.WriteByte(b);
            }
            return Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
         }
      }

      public static void WriteNullTerminatedString(this BinaryWriter writer, string value) {
         var buffer = Encoding.UTF8.GetBytes(value);
         writer.Write(buffer);
         writer.Write((byte)0);
      }

      public static Guid ReadGuid(this BinaryReader reader) {
         return new Guid(reader.ReadBytes(16));
      }

      public static void Write(this BinaryWriter writer, Guid guid) {
         writer.Write(guid.ToByteArray(), 0, 16);
      }

      //http://stackoverflow.com/questions/8613187/an-elegant-way-to-consume-all-bytes-of-a-binaryreader
      public static byte[] ReadAllBytes(this BinaryReader reader) {
         const int bufferSize = 4096;
         using (var ms = new MemoryStream()) {
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
               ms.Write(buffer, 0, count);
            return ms.ToArray();
         }
      }

      /// <summary>
      /// Writes the given tiny text to the binary writer.
      /// TinyText is written as a pascal string with length encoded in 8-bits.
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="s"></param>
      public static void WriteTinyText(this BinaryWriter writer, string s) {
         if (s.Length > 0xFF)
            throw new Exception("Couldn't write the string " + s + " as tinytext, as it was too long");
         else {
            var content = Encoding.UTF8.GetBytes(s);
            writer.Write((byte)s.Length);
            writer.Write(content, 0, s.Length);
         }
      }

      /// <summary>
      /// Writes the given text to the binary writer.
      /// Text is written as a pascal string with length encoded in 16-bits.
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="s"></param>
      public static void WriteText(this BinaryWriter writer, string s) {
         if (s.Length > 0xFFFF)
            throw new Exception("Couldn't write the string " + s + " as text, as it was too long");
         else {
            var content = Encoding.UTF8.GetBytes(s);
            writer.Write((ushort)s.Length);
            writer.Write(content, 0, s.Length);
         }
      }

      /// <summary>
      /// Writes the given text to the binary writer.
      /// LongText is written as a pascal string with length encoded in 32-bits.
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="s"></param>
      public static void WriteLongText(this BinaryWriter writer, string s) {
         // We don't do any range checking, as string.length is a signed integer value,
         // and thusly cannot surpass 2^32 - 1
         var content = Encoding.UTF8.GetBytes(s);
         writer.Write((uint)s.Length);
         writer.Write(content, 0, s.Length);
      }

      /// <summary>
      /// Reads tiny text from the given binary reader.
      /// TinyText is written as a pascal string with length encoded in 8-bits.
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      public static string ReadTinyText(this BinaryReader reader) {
         var length = reader.ReadByte();
         return Encoding.UTF8.GetString(reader.ReadBytes(length), 0, length);
      }

      /// <summary>
      /// Reads text from the given binary reader.
      /// Text is written as a pascal string with length encoded in 16-bits.
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      public static string ReadText(this BinaryReader reader) {
         var length = reader.ReadUInt16();
         return Encoding.UTF8.GetString(reader.ReadBytes(length), 0, length);
      }

      /// <summary>
      /// Reads long text from the given binary reader.
      /// LongText is written as a pascal string with length encoded in 32-bits.
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      public static string ReadLongText(this BinaryReader reader) {
         var length = reader.ReadUInt32();

         if (length > Int32.MaxValue)
            throw new Exception("Attempted to read a string longer than permitted by .net");
         else
            return Encoding.UTF8.GetString(reader.ReadBytes((int)length), 0, (int)length);
      }
   }
}
