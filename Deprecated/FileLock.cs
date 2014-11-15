using System.IO;

namespace ItzWarty.Deprecated
{
   public class FileLock : ReferenceCounter<FileLock>
   {
      private readonly string path;
      private FileStream fileStream;

      public FileLock(string path) { this.path = path; }

      protected override void Initialize()
      {
         while (true) {
            try {
               fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
               break;
            } catch (IOException) {
               // file already open
               Thread.Sleep(100);
               continue;
            }
         }
      }

      protected override void Destroy() { if (fileStream != null) fileStream.Dispose(); }

      protected override FileLock GetExposed() { return this; }
   }
}
