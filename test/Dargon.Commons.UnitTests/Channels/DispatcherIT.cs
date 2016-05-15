using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMockito;
using Xunit;

namespace Dargon.Channels.Tests {
   public class DispatcherIT : NMockitoInstance {
      [Fact]
      public async Task DispatchTwiceTestAsync() {
         var channel = ChannelFactory.Nonblocking<int>();
         await channel.WriteAsync<int>(1);
         await channel.WriteAsync<int>(2);
         await channel.WriteAsync<int>(3);

         var list = new List<int>();
         await Dispatch.Times(2)
                       .Case(channel, i => list.Add(i))
                       .WaitAsync();
         AssertSequenceEquals(new[] { 1, 2 }, list);
      }

      [Fact]
      public async Task DispatchForeverTestAsync() {
         var channel = ChannelFactory.Nonblocking<int>();
         await channel.WriteAsync<int>(1);
         await channel.WriteAsync<int>(2);

         var list = new List<int>();
         Func<int, Task> handler = async (i) => {
            list.Add(i);
         };

         var dispatcher = Dispatch.Forever().Case(channel, handler);

         await Task.Delay(1000);
         AssertSequenceEquals(new[] { 1, 2 }, list);

         await channel.WriteAsync<int>(3);
         await Task.Delay(1000);
         AssertSequenceEquals(new[] { 1, 2, 3 }, list);

         await dispatcher.ShutdownAsync();
         await channel.WriteAsync<int>(4);
         await Task.Delay(1000);
         AssertSequenceEquals(new[] { 1, 2, 3 }, list);
      }
   }
}