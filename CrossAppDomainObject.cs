//To stop all references to this class from working
#define DISABLED 

//From http://nbevans.wordpress.com/2011/04/17/memory-leaks-with-an-infinite-lifetime-instance-of-marshalbyrefobject/ cited as "Public Domain".
//Applying changes to the disposing methods to properly fit the usage of dispose.  This is necessary because of the way C# works - if we are
//finalizing, we should NOT dispose of managed stuff.
//Forced implementation of NestedCrossDomainObjects in order to ensure that coder wouldn't forget about it.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace ItzWarty
{
#if !DISABLED
   /// <summary>
   /// Enables access to objects across application domain boundaries.
   /// This type differs from <see cref="MarshalByRefObject"/> by ensuring that the
   /// service lifetime is managed deterministically by the consumer.
   /// 
   /// If you dispose of the CrossAppDomainObject, every object below the CrossAppDomainObject
   /// will also be disposed.  Doing this simply closes the object's remoting connection.
   /// </summary>
   public abstract class CrossAppDomainObject : MarshalByRefObject
   {
      /// <summary>
      /// Necessary in case we have cirular references
      /// </summary>
      bool m_hasDisconnected = false;

      /// <summary>
      /// Gets an enumeration of nested <see cref="MarshalByRefObject"/> objects.
      /// </summary>
      protected abstract IEnumerable<CrossAppDomainObject> NestedCrossDomainObjects { get; }

      ~CrossAppDomainObject()
      {
         Disconnect(false);
      }

      /// <summary>
      /// Disconnects the CrossAppDomainObject, destroying the lease which the server holds on
      /// it.  While c# doesn't actually use refcounting, you can think of this as pretty much
      /// identical; the CDAO's lease holds a reference to the CDAO.  When that lease is
      /// destroyed, one fewer object knows of the CDAO, bringing us one step closer to being
      /// garbage collected.
      /// 
      /// This method is called recursively; every nested cross domain object below this object
      /// is also released when this method is invoked.
      /// </summary>
      public void Disconnect()
      {
         Disconnect(true);
      }

      /// <summary>
      /// Disconnects the remoting channel(s) of this object and all nested objects.
      /// </summary>
      private void Disconnect(bool userInvoked)
      {
         if (!m_hasDisconnected)
         {
            m_hasDisconnected = true;

            RemotingServices.Disconnect(this);

            //If we're userInvoked, we want to disconnect our children.  It is assumed that userInvoked
            //IDargonServiceMin, for example, will disconnect the Dargon Service Wrapper.
            //If we're not userInvoked and this method is called, then that means that we are being
            //garbage collected.  If that's the case, then we don't disconnect our children, as the
            //client may or may not want to keep references to them.
            //The destructor will only be called after the disconnect method has previously been
            //invoked, so as long as disconnect is called, this is fine.
            if (userInvoked)
            {
               foreach (var tmp in NestedCrossDomainObjects)
                  tmp.Disconnect(true);
            }
         }
      }

      public override object InitializeLifetimeService()
      {
         //
         // Returning null designates an infinite non-expiring lease.
         // We must therefore ensure that RemotingServices.Disconnect() is called when
         // it's no longer needed otherwise there will be a memory leak.
         //
         return null;
      }
   }
#endif
}
