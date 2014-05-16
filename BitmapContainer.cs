using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ItzWarty
{
    /// <summary>
    /// Contains the bitmap.  
    /// Width/Height operations of a bitmap are slow for some reason. This just caches those since
    /// that's static for all the bitmaps i've ever worked with (though you could, technically, subclass from image and
    /// override Width/Height to be dynamic)
    /// </summary>
    public struct BitmapContainer
    {
        public Bitmap bitmap;
        public int width;
        public int height;
        public static BitmapContainer Create(Bitmap bmp)
        {
            return new BitmapContainer()
            {
                bitmap = bmp,
                width = bmp.Width,
                height = bmp.Height
            };
        }
    }
}
