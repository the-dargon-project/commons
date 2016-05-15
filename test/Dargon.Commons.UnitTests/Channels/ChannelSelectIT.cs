using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Nito.AsyncEx;
using NMockito;
using Xunit;
using static Dargon.Channels.ChannelsExtensions;
namespace Dargon.Channels.Tests {
   public class ChannelSelectIT : NMockitoInstance {
      [Fact]
      public void Run() {
         var sw = new Stopwatch();
         for (var i = 0; i < 10; i++) {
            sw.Restart();
            RunTrialAsync().Wait();
            Console.WriteLine($"Trial {i}: {sw.ElapsedMilliseconds} millis.");
         }
      }
      public async Task RunTrialAsync() {
         var channel1 = ChannelFactory.Nonblocking<int>();
         var channel2 = ChannelFactory.Blocking<int>();
         var barrier = new AsyncBarrier(2);
         var semaphore = new AsyncSemaphore(0);
         Go(async () => {
            for (var i = 0; i < 1000; i++) {
               await barrier.SignalAndWaitAsync();
               await semaphore.WaitAsync();
               await channel1.WriteAsync(i);
            }
         });
         Go(async () => {
            for (var i = 0; i < 1000; i++) {
               await barrier.SignalAndWaitAsync(); 
               await semaphore.WaitAsync();
               await channel2.WriteAsync(i);
            }
         });
         int counter1 = 0;
         int counter2 = 0;
         for (var i = 0; i < 1000; i++) {
            semaphore.Release(2);
            for (var j = 0; j < 2; j++) {
               await Select.Case(channel1, val => {
                  AssertEquals(counter1, val);
                  counter1++;
               }).Case(channel2, val => {
                  AssertEquals(counter2, val);
                  counter2++;
               }).WaitAsync();
            }
         }
      }
   }
}
