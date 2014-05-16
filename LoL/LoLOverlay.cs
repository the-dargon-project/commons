using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace ItzWarty.LoL
{
    public class LoLOverlay
    {
        protected Bitmap bitmap = null;
        protected Graphics g = null;
        protected Point location = new Point(0, 0);

        private OverlayForm form = new OverlayForm();
        public LoLOverlay(Bitmap bmp)
        { 
            this.bitmap = bmp; 
            this.g = Graphics.FromImage(bmp); 
        }
        public void Render()
        {
            g.FillRectangle(Brushes.Purple,
                0, 0, bitmap.Width, bitmap.Height
            );
        }

        public void UpdateOverlay()
        {
            //Console.WriteLine("Update: " + this.location);
            form.Update(this.bitmap, this.location);
            form.Show();
        }
        public void HideOverlay()
        {
            //form.Hide();
        }
        public void SetLocation(Point loc) { this.location = loc;}
        public void SetLocationRelativeToGame(Point loc)
        {
            Rectangle lolRect = WinAPI.GetWindowRect(LoLAnalyzer.GetLoLHandle());
            int titleBarHeight = System.Windows.Forms.SystemInformation.CaptionHeight;
            int borderWidth = System.Windows.Forms.SystemInformation.SizingBorderWidth;

            if (LoLAnalyzer.GetLoLState() == LoLState.AirClient)
            {
                SetLocation(
                    new Point(
                        loc.X + lolRect.Left,
                        loc.Y + lolRect.Top
                    )
                );
            }
            else
            {
                SetLocation(
                    new Point(
                        lolRect.Left + borderWidth + loc.X,
                        lolRect.Top + titleBarHeight + loc.Y
                    )
                );
            }
        }
        public void SetTopRelativeToGame(int value)
        {
            Rectangle lolRect = WinAPI.GetWindowRect(LoLAnalyzer.GetLoLHandle());
            int titleBarHeight = System.Windows.Forms.SystemInformation.CaptionHeight;
            int borderWidth = System.Windows.Forms.SystemInformation.SizingBorderWidth;

            if (LoLAnalyzer.GetLoLState() == LoLState.AirClient)
            {
                SetLocation(
                    new Point(this.location.X, value)
                );
            }
            else
            {
                SetLocation(
                    new Point(
                        this.location.X,
                        lolRect.Top + titleBarHeight + value
                    )
                );
            }
        }
        public void SetBottomRelativeToGame(int value)
        {
            Rectangle lolRect = WinAPI.GetWindowRect(LoLAnalyzer.GetLoLHandle());
            int titleBarHeight = System.Windows.Forms.SystemInformation.CaptionHeight;
            int borderWidth = System.Windows.Forms.SystemInformation.SizingBorderWidth;

            if (LoLAnalyzer.GetLoLState() == LoLState.AirClient)
            {
                SetLocation(
                    new Point(this.location.X, lolRect.Bottom - value - this.bitmap.Height)
                );
            }
            else
            {
                SetLocation(
                    new Point(
                        this.location.X,
                        lolRect.Bottom - borderWidth - value - this.bitmap.Height
                    )
                );
            }
        }
        public void SetLeftRelativeToGame(int value)
        {
            Rectangle lolRect = WinAPI.GetWindowRect(LoLAnalyzer.GetLoLHandle());
            int titleBarHeight = System.Windows.Forms.SystemInformation.CaptionHeight;
            int borderWidth = System.Windows.Forms.SystemInformation.SizingBorderWidth;

            if (LoLAnalyzer.GetLoLState() == LoLState.AirClient)
            {
                SetLocation(
                    new Point(value, this.location.Y)
                );
            }
            else
            {
                SetLocation(
                    new Point(
                        lolRect.Left + borderWidth + value,
                        this.location.Y
                    )
                );
            }
        }
        public void SetRightRelativeToGame(int value)
        {
            Rectangle lolRect = WinAPI.GetWindowRect(LoLAnalyzer.GetLoLHandle());
            int titleBarHeight = System.Windows.Forms.SystemInformation.CaptionHeight;
            int borderWidth = System.Windows.Forms.SystemInformation.SizingBorderWidth;

            if (LoLAnalyzer.GetLoLState() == LoLState.AirClient)
            {
                SetLocation(
                    new Point(lolRect.Right - value - this.bitmap.Width, this.location.Y)
                );
            }
            else
            {
                SetLocation(
                    new Point(
                        lolRect.Right - borderWidth - value - this.bitmap.Width,
                        this.location.Y
                    )
                );
            }
        }
    }
}
