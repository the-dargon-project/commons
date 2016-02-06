using System;

namespace Dargon.Commons
{
   public abstract class ReferenceCounter<TExposed> : IDisposable
   {
      private readonly object bigLock = new object();
      private int referenceCount = 0;

      public TExposed Take()
      {
         lock (bigLock) {
            if (referenceCount == 0) {
               Initialize();
            }
            referenceCount++;
         }
         return GetExposed();
      }

      private void Return()
      {
         lock (bigLock) {
            referenceCount--;
            if (referenceCount == 0) {
               Destroy();
            }
         }
      }

      protected abstract void Initialize();
      protected abstract void Destroy();
      protected abstract TExposed GetExposed();

      public void Dispose() { Return(); }
   }
}
