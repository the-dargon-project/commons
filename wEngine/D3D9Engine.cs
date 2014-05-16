using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.Windows.Forms;

using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;

using D3D = SlimDX.Direct3D9;

namespace ItzWarty.wEngine
{
    //public enum GraphicsDriver driver 
    public class D3D9Engine : Node, IGraphicsEngine
    {
        private Form form = null;
        private Device device = null;
        private D3D.Font systemFont = null;
        private Sprite systemFontSprite = null;

        private List<IVertexBufferObject> vbos = new List<IVertexBufferObject>();

        /// <summary>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="windowed"></param>
        /// <param name="bpp">Ignored</param>
        /// <param name="depth">Ignored</param>
        /// <param name="title"></param>
        public D3D9Engine(int width, int height, bool windowed, int bpp, int depth, string title)
        {
            this.form = new Form();
            if (windowed)
            {
                form.ClientSize = new Size(width, height);
            }
            else
            {
                form.ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            }
            form.Text = title;

            form.Load += new EventHandler(form_Load);
            form.ResizeEnd += new EventHandler(ResetDevice);
            device = new Device(new Direct3D(), 0, DeviceType.Hardware, form.Handle, 
                CreateFlags.SoftwareVertexProcessing, this.CreatePresentParameters());
            SetupDevice();
        }

        public event OnLoadHandler Load = null;
        void form_Load(object sender, EventArgs e)
        {
            if (this.Load != null) this.Load();
        }

        public void Reset()
        {
            lock (device)
            {
                try
                {
                    vbos[0].PreDeviceReset();

                    device.Reset(CreatePresentParameters());

                    vbos[0].PostDeviceReset();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                //SetupDevice();
            }
        }
        private void ResetDevice(object sender, EventArgs e) { Reset(); }

        private void SetupDevice()
        {
            /*
            systemFont = new D3D.Font(device, 18, 0, FontWeight.Normal, 0, false, CharacterSet.Hangul, Precision.Default,
                        FontQuality.Antialiased, PitchAndFamily.Default, "Lucida Console");
            systemFontSprite = new Sprite(device);
             */
        }

        private PresentParameters CreatePresentParameters()
        {
            PresentParameters pp = new PresentParameters();
            //pp.BackBufferCount = 2;
            pp.BackBufferFormat = Format.X8R8G8B8;
            pp.BackBufferHeight = form.ClientSize.Height;
            pp.BackBufferWidth = form.ClientSize.Width;
            pp.SwapEffect = SwapEffect.Discard;
            pp.Windowed = true;
            return pp;
        }

        public IVertexBufferObject CreateVertexBuffer<F>(int length) where F:IVertex
        {
            IVertexBufferObject vbo = new D3D9VertexBufferObject<F>(this.device, length);
            vbos.Add(vbo);
            return vbo;
        }

        public void SetStreamSource<E>(int n, IVertexBufferObject vbo)
            where E:IVertex
        {
            device.SetStreamSource(n, ((D3D9VertexBufferObject<E>)vbo).GetVertexBuffer(), 0, ((D3D9VertexBufferObject<E>)vbo).GetVerticesStride());
        }

        public event OnRenderFrameHandler RenderFrame;
        public void Run()
        {
            MessagePump.Run(form, () =>
                {
                    lock (device)
                    {
                        if (RenderFrame != null) RenderFrame();
                        /*
                        device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new Color4(Color.Black), 1.0f, 0);
                        device.BeginScene();

                        systemFontSprite.Begin(SpriteFlags.AlphaBlend);
                        systemFont.DrawString(systemFontSprite, "Hello World", 0, 0, new Color4(Color.White));
                        systemFontSprite.End();

                        device.EndScene();
                        device.Present();
                         */
                    }
                }
            );
        }
        public void Clear(Color c)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new Color4(c), 1.0f, 0);
        }
        public void BeginScene()
        {
            device.BeginScene();
        }
        public void EndScene()
        {
            device.EndScene();
        }
        public void Present()
        {
            device.Present();
        }
        public void Dispose()
        {
            systemFontSprite.Dispose();
            systemFont.Dispose();
            device.Dispose();
        }
    }
}
