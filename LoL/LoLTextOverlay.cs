using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace ItzWarty.LoL
{
    public class LoLTextOverlay:LoLOverlay
    {
        private Color foregroundColor = Color.White;
        private Color backgroundColor = Color.Black;
        private Brush foregroundBrush = Brushes.White;
        private Brush backgroundBrush = Brushes.Black;
        private string text = "";
        private Font font             = new Font("Lucida Console", 8f);

        public LoLTextOverlay(Bitmap bmp)
            : base(bmp)
        {
        }
        public void SetText(string s)
        {
            this.text = s;
        }
        public void SetColor(Color c)
        {
            this.foregroundColor = c;
            this.foregroundBrush.Dispose();
            this.foregroundBrush = new SolidBrush(this.foregroundColor);
        }
        public void SetBackgroundColor(Color c)
        {
            this.backgroundColor = c;
            this.backgroundBrush.Dispose();
            this.backgroundBrush = new SolidBrush(this.backgroundColor);
        }
        public new void Render()
        {
            if (this.text == "")
            {
                this.HideOverlay();
            }
            else
            {
                Size textSize = g.MeasureString(this.text, this.font).ToSize();
                if (!this.bitmap.Size.Equals(text))
                {
                    this.g.Dispose();
                    this.bitmap.Dispose();
                    this.bitmap = new Bitmap(textSize.Width, textSize.Height);
                    this.g = Graphics.FromImage(this.bitmap);
                }

                this.g.FillRectangle(
                    this.backgroundBrush,
                    0, 0, this.bitmap.Width, this.bitmap.Height
                );

                g.DrawString(this.text, this.font, this.foregroundBrush, new Point(0, 0));

                this.UpdateOverlay();
            }
        }
    }
}
