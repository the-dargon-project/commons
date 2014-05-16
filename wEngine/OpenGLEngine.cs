using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ItzWarty.wEngine
{
    //public enum GraphicsDriver driver 
    public class OpenGLEngine : Node, IGraphicsEngine
    {
        private GameWindow form = null;
        public event OnLoadHandler Load;

        //800, 600, true, 32, 8, "Hello World"
        public OpenGLEngine(int width, int height, bool windowed, int bpp, int depth, string title)
        {
            this.form = new GameWindow(width, height, new GraphicsMode(new ColorFormat(bpp), depth), title);
            form.Resize += new EventHandler<EventArgs>(form_Resize);
            form.RenderFrame += new EventHandler<FrameEventArgs>(form_RenderFrame);
            form.Load += new EventHandler<EventArgs>(form_Load);
            SetupDevice();
        }

        void form_Load(object sender, EventArgs e)
        {
            if (this.Load != null)
                this.Load();
        }

        public event OnRenderFrameHandler RenderFrame;
        void form_RenderFrame(object sender, FrameEventArgs e)
        {
            //GL.ClearColor(Color.Black);
            if (this.RenderFrame != null) this.RenderFrame();
            //form.SwapBuffers();
        }

        void form_Resize(object sender, EventArgs e)
        {
            //Adjust ogl viewpoint & backbuffer size
        }

        public void Reset()
        {
        }
        private void ResetDevice(object sender, EventArgs e) { Reset(); }

        private void SetupDevice()
        {
        }

        public IVertexBufferObject CreateVertexBuffer<F>(int length) where F : IVertex
        {
            
            return new OpenGLVertexBufferObject<F>(length);
        }
        public void SetStreamSource<E>(int n, IVertexBufferObject vbo)
            where E : IVertex
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ((OpenGLVertexBufferObject<E>)vbo).VboID);
            //device.SetStreamSource(n, ((D3D9VertexBufferObject<E>)vbo).GetVertexBuffer(), 0, ((D3D9VertexBufferObject<E>)vbo).GetVerticesStride());
        }

        public void Run()
        {
            form.Run();
        }
        public void Clear(Color c)
        {
            GL.ClearColor(c);
        }
        public void BeginScene() { }

        public void EndScene()
        {
            DisableVSync();
            GL.Flush(); //Forces execution of draw commands
            GL.Finish();
        }

        #region wglGetProcAddress([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string param0)
        //http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/924809d8-6fe8-460e-8bd5-e8f79898e357/
        //http://www.devmaster.net/forums/showthread.php?t=443
        public delegate int PROC(int k);
        [System.Runtime.InteropServices.DllImportAttribute("opengl32.dll", EntryPoint = "wglGetProcAddress")]
        public static extern PROC wglGetProcAddress([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string param0);
        #endregion
        public void DisableVSync()
        {
            if (GL.GetString(StringName.Extensions).IndexOf("WGL_EXT_swap_control") != -1)
            {
                wglGetProcAddress("wglSwapIntervalEXT")(0);
            }
        }
        public void Present()
        {
            form.SwapBuffers();
        }
        public void Dispose()
        {
        }
    }
}
