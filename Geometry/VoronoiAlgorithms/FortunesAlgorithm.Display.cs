using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ItzWarty.Geometry.Displays;

namespace ItzWarty.Geometry.VoronoiAlgorithms
{
   public partial class FortunesAlgorithm
   {
      public static void Main()
      {
         new FortunesAlgorithm(new Point2D[] { new Point2D(100, 100), new Point2D(150, 130), new Point2D(50, 160)  }).ShowDisplay();
         Application.Run();
      }

      private void ShowDisplay()
      {
         var plot = new Plot2D(0, 300, 0, 300, 2.0f);

         this.RunIteration(2);

         plot.Draw(new Line2D(new Point2D(0, m_lineY), new Vector2D(1000, 0)), Pens.LightGray);

         foreach (var point in m_initialPoints)
            plot.Draw(point, Brushes.LightGray);

         while (!m_queue.Empty) {
            var e = m_queue.Next();
            if (e.EventType == VEventType.Site) {
               plot.Draw(e.Site, Brushes.Lime);
            }
            else {
               var ce = (VCircleEvent)e;
               plot.Draw(ce.CircleCenter, Brushes.Magenta);
            }
         }
         
         if(m_root != null)
         {
            m_root.Recursively(
               SeedInclusion.Include,
               (node) => {
                  if (node is VParabolaNode)
                  {
                     Console.WriteLine("PARABOLA");
                     plot.Draw(node.Site, Brushes.Red);

                     if (Math.Abs(node.Site.Y - m_lineY) <= kEqualityEpsilon) {

                     } else {
                        var parabola2d = new Parabola2D(node.Site, new Line2D(new Point2D(0, m_lineY), new Vector2D(3, 0)));
                        Console.WriteLine(parabola2d.PointAtT(0));
                        plot.DrawParabola(parabola2d);
                     }
                  }
                  else
                  {
                     Console.WriteLine("EDGE");
                     var edgeNode = (VEdgeNode)node;
                     plot.Draw(node.Site, Brushes.Blue);
                     plot.Draw(new Line2D(node.Site, edgeNode.Direction), Pens.Cyan);
                  }
               },
               node => {
                  var result = new List<VNode>(2);
                  if(node.Left != null)
                     result.Add(node.Left);
                  if (node.Right != null)
                     result.Add(node.Right);
                  return result;
               }
            );
         }

         new Display2D(plot).Show();
      }
   }
}
