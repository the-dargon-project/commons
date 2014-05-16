using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ItzWarty.LoL
{
    public partial class OverlayForm : Form
    {
        PictureBox pb = new PictureBox();
        public OverlayForm()
        {
            InitializeComponent();

            //http://stackoverflow.com/questions/357076/best-way-to-hide-a-window-from-the-alt-tab-program-switcher
            ShowInTaskbar = false;

            Form form1 = new Form();

            form1.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            form1.ShowInTaskbar = false;

            Owner = form1;

            pb.Dock = DockStyle.Fill;
            pb.MinimumSize = new Size(0, 0);
            pb.MaximumSize = new Size(0xFFFFFF, 0xFFFFFF);
            this.Controls.Add(pb);
            pb.Show();
        }
        public void Update(Bitmap bmp, Point location)
        {
            this.pb.Image = bmp;
            WinAPI.MoveWindow(
                this.Handle,
                location.X, location.Y, bmp.Width, bmp.Height, true
            );
            this.pb.Location = new Point(0, 0);
            this.BringToFront();
            this.TopMost = true;
        }

        private int WM_NCHITTEST = 0x0084;
        private int HTTRANSPARENT = -1;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }
}
