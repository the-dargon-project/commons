using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ItzWarty;

namespace ItzWarty
{
   public static partial class Extensions
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static PointF ToPointF(this Point point)
      {
         return new PointF(point.X, point.Y);
      }

      /// <summary>
      /// Convers the given color to the HTML Representation of a color, #RRGGBB
      /// </summary>
      public static string ToHex(this Color c)
      {
         return "#" + c.R.ToString("X").PadLeft(2, '0')
                    + c.G.ToString("X").PadLeft(2, '0')
                    + c.B.ToString("X").PadLeft(2, '0');
      }
   }
}
