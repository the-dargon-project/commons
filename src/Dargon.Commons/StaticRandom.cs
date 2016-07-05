using System;

namespace Dargon.Commons
{
   public static class StaticRandom {
      [ThreadStatic] private static Random random;

      public static Random Random => random ?? (random = new Random());


      public static int Next(int exclusiveUpperBound)
      {
            return Random.Next(exclusiveUpperBound);
      }

      public static int Next(int inclusiveLowerBound, int exclusiveUpperBound)
      {
            return Random.Next(inclusiveLowerBound, exclusiveUpperBound);
      }

      public static float NextFloat(float exclusiveUpperBound)
      {
            return (float)NextDouble(exclusiveUpperBound);
      }

      public static float NextFloat(float inclusiveLowerBound, float exclusiveUpperBound)
      {
            return (float)NextDouble(inclusiveLowerBound, exclusiveUpperBound);
      }

      public static double NextDouble()
      {
            return Random.NextDouble();
      }

      public static double NextDouble(double exclusiveUpperBound)
      {
            return Random.NextDouble() * exclusiveUpperBound;
      }

      public static double NextDouble(double inclusiveLowerBound, double exclusiveUpperBound)
      {
            return inclusiveLowerBound + Random.NextDouble() * (exclusiveUpperBound - inclusiveLowerBound);
      }

      public static Random NextRandom()
      {
         var buffer = new byte[4];
         Random.NextBytes(buffer);
         return new Random(BitConverter.ToInt32(buffer, 0));
      }

      public static bool NextBoolean()
      {
            return Random.Next() % 2 == 0;
      }
   }
}
